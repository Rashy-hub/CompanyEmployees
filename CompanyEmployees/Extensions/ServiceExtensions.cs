using AspNetCoreRateLimit;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using LoggerService;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Service;
using Service.Contracts;
using System.Text;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {

            services.Configure<IISOptions>(options =>
            {
                //default values are ok for now
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }
        // la methode suivante est trés importante pour chargé le context sql au runtime pour pouvoir l'injecter (que RepositoryManager utilise)
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            //AddSqlServer remplace AddDbContext + UseSqlServer mais est moins flexible en options differentes
            services.AddSqlServer<RepositoryContext>(configuration.GetConnectionString("sqlConnection"));

        }

        public static IMvcBuilder AddCustomOutputFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions((config) => config.OutputFormatters.Add(new CsvOutputFormatter()));
        }
        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders((expirationOptions) =>
            {
                expirationOptions.MaxAge = 65;
                expirationOptions.CacheLocation = CacheLocation.Public;
            }, (validationModelOptions) =>
            {
                validationModelOptions.MustRevalidate = true;
            },
             (middlewareOptions) =>
             {
                 middlewareOptions.IgnoredStatusCodes = new[] { 500 };
             });
        }

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>{
                                //demo values, not for production
                                    new RateLimitRule
                                    {
                                    Endpoint = "*",
                                    Limit = 20,
                                    Period = "5m"
                                    }
                                };
            services.Configure<IpRateLimitOptions>(opt => opt.GeneralRules = rateLimitRules);
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 10;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();
        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
           
            var jwtConfiguration = new JwtConfiguration();
            configuration.GetSection(JwtConfiguration.Section).Bind(jwtConfiguration);           
          
            var secretKey = jwtConfiguration.Secret;

            if (string.IsNullOrWhiteSpace(secretKey))
                throw new Exception("JWT Secret is missing");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfiguration.ValidIssuer,
                    ValidAudience = jwtConfiguration.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

    }
}
