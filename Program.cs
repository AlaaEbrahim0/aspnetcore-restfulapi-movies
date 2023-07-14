using System.ComponentModel;
using System.Text;
using System.Transactions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoviesApi.Helpers;
using MoviesApi.Services;
using MoviesApi.Services.Contracts;

namespace WebApplication1
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			// Add services to the container.

			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddAutoMapper(typeof(Program));

			builder.Services.AddTransient<IGenresService, GenresService>();
			builder.Services.AddTransient<IMoviesService, MoviesService>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<Jwt>();


			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireDigit = true;
			})
				.AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));

			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder.AllowAnyOrigin()
						   .AllowAnyMethod()
						   .AllowAnyHeader();
				});
			});


			builder.Services.AddAuthentication(options =>
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

					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Audience"],

					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
				};
			});


			builder.Services.AddSwaggerGen(options =>
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


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseCors();

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
			
		}
	}
}