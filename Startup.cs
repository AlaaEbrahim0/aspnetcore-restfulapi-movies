using Microsoft.AspNetCore.Hosting;
using MoviesApi.Extensions;

namespace MoviesApi;

public class Startup
{
	private readonly IConfiguration _configuration;

	public Startup(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{

		ServiceExtensions.ConfigureCors(services);
		ServiceExtensions.ConfigureIdentity(services);
		ServiceExtensions.ConfigureAuthentication(services, _configuration);
		ServiceExtensions.ConfigureDbContext(services, _configuration);

		services.AddControllers();

		services.AddEndpointsApiExplorer();

		services.AddAutoMapper(typeof(Startup));

		services.AddTransient<IGenresService, GenresService>();
		services.AddTransient<IMoviesService, MoviesService>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<Jwt>();

		services.Configure<Jwt>(_configuration.GetSection("Jwt"));

		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "MoviesApi",
				Description = "Test Description",
				Contact = new OpenApiContact
				{
					Name = "Alaa Ebrahim",
					Email = "alaaebrahim387@gmail.com",
				}

			});
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
			});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						},
						Name = "Bearer",
						In = ParameterLocation.Header,

					},
					new List<string>()
				}
			});
		});

	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseCors("CorsPolicy");

		app.UseRouting();

		app.UseCors("CorsPolicy");

		app.UseAuthentication();

		app.UseAuthentication();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}