using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using CleanArchitectureTemplate.Application.Common.EmailServices;
using CleanArchitectureTemplate.Application.Common.IdentityServices;
using CleanArchitectureTemplate.Domain.Customers.Interfaces;
using CleanArchitectureTemplate.Infrastructure.Persistence;
using CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;
using CleanArchitectureTemplate.Infrastructure.Services.IdentityServices;
using CleanArchitectureTemplate.Infrastructure.Services.EmailServices;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using System.Text;
using Hangfire;
using Newtonsoft.Json;
using CleanArchitectureTemplate.Application.Common.BackgroundJobs;
using CleanArchitectureTemplate.Infrastructure.Services.BackgroundJobs;
using CleanArchitectureTemplate.Application.Common.FileStorageServices;
using CleanArchitectureTemplate.Infrastructure.Services.FileStorageServices;
using CleanArchitectureTemplate.Infrastructure.Persistence.Models.RefreshTokens.Interfaces;
using System.Reflection;
using CleanArchitectureTemplate.Infrastructure.Common;
using CleanArchitectureTemplate.Application.Common.EmailTemplates;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Builder;

namespace CleanArchitectureTemplate.Infrastructure
{
	public static class ConfigureDependencies
	{
		public static void AddPersistenceDependancies(this IServiceCollection services, IConfiguration configuration)
		{
			var dbConnectionString = configuration.GetConnectionString("DbConnectionString");
			services.AddDbContextPool<EFDbContext>(o => o.UseSqlServer(dbConnectionString));

			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
		}

		public static void AddServicesDependancies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IInteractorExecution, InteractorExecution>();
			
			services.Configure<SendGridConfiguration>(configuration.GetSection(SendGridConfiguration.ConfigurationName));
			services.AddSingleton<IEmailService, SendGridService>();

			services.AddSingleton<IFileStorageService, AzureFileStorageService>();

			services.AddSingleton<IBackgroundScheduleJob, BackgroundScheduleJob>();

			services.AddTransient<IEmailTemplateHelper, EmailTemplateHelper>();

		}
		
		public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = false,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = configuration["Jwt:Issuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
					};
				});

			services.AddScoped<ITokenService, BearerTokenService>();
		}
		
		public static void AddHangeFire(this IServiceCollection services, IConfiguration configuration)
		{
			var dbConnectionString = configuration.GetConnectionString("DbConnectionString");

			services.AddHangfire(configuration => configuration
				.UseSimpleAssemblyNameTypeSerializer()
				.UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
				.UseSqlServerStorage(dbConnectionString));

			services.AddHangfireServer();
		}
		
		public static void AddSwagger(this IServiceCollection services)
		{
			var xmlFilename = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly().GetName().Name}.xml");
			services.AddSwaggerGen(options =>
			{
				options.IncludeXmlComments(xmlFilename);
				options.UseInlineDefinitionsForEnums();
				options.SchemaFilter<CustomDateOnlySchemaFilter>();
			});

            services.Configure<SwaggerUIOptions>(options =>
            {
                options.DocExpansion(DocExpansion.None);
            });
        }
	}
}
