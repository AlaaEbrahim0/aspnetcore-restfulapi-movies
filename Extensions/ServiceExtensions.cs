namespace MoviesApi.Extensions;

public static class ServiceExtensions
{
	public static void ConfigureCors(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", policy =>
			{
				policy
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowAnyOrigin();
			});
		});
	}
	public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
		});
	}
	public static void ConfigureIdentity(this IServiceCollection services)
	{
		services.AddIdentity<ApplicationUser, IdentityRole>(options =>
		{
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireLowercase = true;
			options.Password.RequireUppercase = true;
			options.Password.RequireDigit = true;
		})
			.AddEntityFrameworkStores<ApplicationDbContext>();
	}
	public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

		})
		.AddJwtBearer(opt =>
		{
			opt.RequireHttpsMetadata = false;
			opt.SaveToken = false;

			opt.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,

				ValidIssuer = configuration["Jwt:Issuer"],
				ValidAudience = configuration["Jwt:Audience"],

				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
			};
		});
	}
}
