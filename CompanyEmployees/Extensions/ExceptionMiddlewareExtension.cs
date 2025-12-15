using Contracts;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace CompanyEmployees.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler (this WebApplication app,ILoggerManager logger)
        {
            app.UseExceptionHandler(appError => {
                appError.Run(async context =>
            {
               // context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    //pattern matching
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException=> StatusCodes.Status400BadRequest,
                        UnprocessableException => StatusCodes.Status422UnprocessableEntity,
                        _ => StatusCodes.Status500InternalServerError
                    };
                    logger.LogError($"Something Went Wrong : {contextFeature.Error}");

                    await context.Response.WriteAsync(new ErrorDetail()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message=contextFeature.Error.Message,
                        Errors = contextFeature.Error is UnprocessableException ue ? ue.Errors : null

                    }.ToString());
                    
                }

            });
            });

            
        }
    }
}
