using Contracts;
using LoggerService;
using Repository;
using Service;
using Service.Contracts;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",builder=>builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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
            services.AddScoped<IRepositoryManager,RepositoryManager>();
        }

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager,ServiceManager>();
        }
        // la methode suivante est trés importante pour chargé le context sql au runtime pour pouvoir l'injecter (que RepositoryManager utilise)
        public static void ConfigureSqlContext (this  IServiceCollection services, IConfiguration configuration)
        {
            //AddSqlServer remplace AddDbContext + UseSqlServer mais est moins flexible en options differentes
            services.AddSqlServer<RepositoryContext>(configuration.GetConnectionString("sqlConnection"));

        }

        public static IMvcBuilder AddCustomOutputFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions((config) => config.OutputFormatters.Add(new CsvOutputFormatter()));
        }
    }
}
