using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Helpers;
using MoviesApi.Services.Contracts;

namespace MoviesApi.Services
{
	public class AuthService : IAuthService
	{
        private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
        private readonly Jwt jwt;
		private readonly IMapper mapper;

		public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper,
			RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IOptions<Jwt> jwt)
		{
			this.userManager = userManager;
			this.mapper = mapper;
			this.jwt = jwt.Value;
			this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public async Task<string> AddRoleAsync(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
			if (role is not null)
				return $"Role: {roleName} already exists";

			var result = await roleManager.CreateAsync(new IdentityRole(roleName));
			if (!result.Succeeded)
				return "Error occured while creating a new role";

			return string.Empty;
        }

        public async Task<string> AddUserToRoleAsync(AddUserToRoleDto dto)
		{
			var user = await userManager.FindByIdAsync(dto.UserId);
			if (user is null)
				return "Invalid User";

			var userAlreadyInRole = await userManager.IsInRoleAsync(user, dto.RoleName);

			if (userAlreadyInRole)
				return "User Already In Role";

			var validRoleName = await roleManager.RoleExistsAsync(dto.RoleName);

			if (!validRoleName)
				return "Invalid Role Name";

			var result = await userManager.AddToRoleAsync(user, dto.RoleName);
			if (!result.Succeeded)
				return string.Join("\n", result.Errors.Select(x => x.Description));

			return string.Empty;
			

		}

        public async Task<AuthDto> GetTokenAsync(TokenRequestDto dto)
		{

			var user = await userManager.FindByEmailAsync(dto.Email);
			if (user is null)
			{
				return new AuthDto() { Message = $"No user with {dto.Email} exists" };
			}

			var validPassword = await userManager.CheckPasswordAsync(user, dto.Password);
			if (!validPassword)
			{
				return new AuthDto() { Message = $"Invalid Password" };
			}

			var jwtSecurityToken = await CreateJwtToken(user);
			
			var userRoles = await userManager.GetRolesAsync(user);

			var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

			return new AuthDto
			{
				Email = user.Email,
				UserName = user.UserName,
				Token = token,
				ExpiresOn = jwtSecurityToken.ValidTo,
				IsAuthenticated = true,
				Roles = userRoles.ToList()
			};
		}

        public async Task SignOutAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                await signInManager.SignOutAsync();
            }
        }

        public async Task<AuthDto> RegisterAsync(RegisterDto dto)
		{
			var userEmail = await userManager.FindByEmailAsync(dto.Email);
			var userName = await userManager.FindByNameAsync(dto.Email);

			if (userEmail is not null) 
				return new AuthDto{	Message = "An account with same email already exists!"};
			
			if (userName is not null)
				return new AuthDto { Message = "An account with same username already exists!" };
		
			var user = mapper.Map<ApplicationUser>(dto);

			var result = await userManager.CreateAsync(user, dto.Password);

			if (!result.Succeeded)
				return new AuthDto { Message = string.Join("\n", result.Errors.Select(x => x.Description)) };
			
			await userManager.AddToRoleAsync(user, Roles.User.ToString());

			var jwtSecurityToken = await CreateJwtToken(user);

			return new AuthDto
			{
				UserName = user.UserName,
				Email = user.Email,
				Roles = new List<string> { Roles.User.ToString() },
				IsAuthenticated = true,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken).ToString(),
				ExpiresOn = jwtSecurityToken.ValidTo
			};

		}

        private async Task<JwtSecurityToken> CreateJwtToken (ApplicationUser user)
		{
			var userClaims = await userManager.GetClaimsAsync(user);
			var userRoles  = await userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in userRoles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				//99l999
				new Claim("uid", user.Id)
			}
			.Union(userClaims)
			.Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

			var signingCredentials	 = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var expirationDate = DateTime.Now.AddDays(jwt.DurationInDays);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: jwt.Issuer,
				audience: jwt.Audience,
				claims: claims,
				expires: expirationDate,
				signingCredentials: signingCredentials
				);

			return jwtSecurityToken;
		}
	}
}
