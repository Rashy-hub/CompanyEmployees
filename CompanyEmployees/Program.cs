using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service.MapperProfiles;


var builder = WebApplication.CreateBuilder(args);


// Load NLog configuration

LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));



// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.ReturnHttpNotAcceptable = true;
   // options.CacheProfiles.Add("DefaultCacheProfile", new CacheProfile { Duration = 120 , Location=ResponseCacheLocation.Client, VaryByQueryKeys = new[] { "pageNumber", "pageSize" } });
})
.AddNewtonsoftJson()                     // nécessaire pour JSON Patch
.AddNewtonsoftJsonPatchSupport()         // methode d'extension patch document formatter
.AddXmlDataContractSerializerFormatters()
.AddCustomOutputFormatter()
.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);


builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.ConfigureCors();

builder.Services.ConfigureIISIntegration();

builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);
//this is internally used by AspNetCoreRateLimit
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureRepositoryManager();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MapperConfig)), typeof(MapperConfig).Assembly); // scan de l'assembly);

builder.Services.ConfigureServiceManager();

var app = builder.Build();



// Get the logger service required for our Exception Handler 

var logger = app.Services.GetRequiredService<ILoggerManager>();
logger.LogInfo("Logger Loaded Successfully");
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All,
});

app.UseCors("CorsPolicy");
    app.UseResponseCaching();
    app.UseHttpCacheHeaders();

app.UseAuthorization();

app.MapControllers();

app.Run();
