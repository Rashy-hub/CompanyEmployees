using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace CompanyEmployees.Extensions
{
    public static class NewtonsoftExtensions
    {
        //This extension method configures support for JSON Patch using Newtonsoft.Json while leaving the other formatters unchanged.
        public static IMvcBuilder AddNewtonsoftJsonPatchSupport(
            this IMvcBuilder builder)
        {
            builder.Services.Configure<MvcOptions>(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            });

            return builder;
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return serviceProvider
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value.InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
