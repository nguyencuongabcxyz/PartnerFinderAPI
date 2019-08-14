using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Service.Models;
using System.Net;

namespace PartnerFinder.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var errorMessage = contextFeature.Error.Message;
                        await context.Response.WriteAsync(new ErrorDetailDto()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = errorMessage
                        }.ToString());
                    }
                });
            });
        }
    }
}
