using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Helpers;
using MoviesApi.Services.Contracts;

namespace MoviesApi.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class AuthController: ControllerBase
	{
		private readonly IAuthService authService;

		public AuthController(IAuthService authService)
        {
			this.authService = authService;
		}

        [HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<AuthDto>> RegisterAsync ([FromBody] RegisterDto dto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(dto);
				}
				
				var result = await authService.RegisterAsync(dto);
				if (!result.IsAuthenticated)
				{
					return BadRequest(result.Message);
				}

				return Ok(result);

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost]
        [AllowAnonymous]

        public async Task<ActionResult<AuthDto>> LoginAsync([FromBody] TokenRequestDto dto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(dto);
				}

				var result = await authService.GetTokenAsync(dto);
				if (!result.IsAuthenticated)
				{
					return BadRequest(result.Message);
				}

				return Ok(result);

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<string>> AddRoleAsync([Required] string roleName)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(roleName);

				var result = await authService.AddRoleAsync(roleName);
				if (!string.IsNullOrEmpty(result))
					return BadRequest(result);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult<string>> AddUserToRoleAsync (AddUserToRoleDto dto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(dto);

				var result = await authService.AddUserToRoleAsync(dto);
				if (!string.IsNullOrEmpty(result))
				{
					return BadRequest(result);
				}

				return Ok(dto);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[Authorize]
        [HttpPost()]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await authService.SignOutAsync(userId);
            }
            return Ok();
        }
    }
}
