using AspNetCoreRateLimit;
using Contracts;
using LoggerService;
using Marvin.Cache.Headers;
using Repository;
using Service;
using Service.Contracts;

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
            services.AddSingleton<IRateLimitCounterStore,MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }
    }
}
