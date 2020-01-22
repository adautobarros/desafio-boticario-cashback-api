using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Boticario.Cashback.Api.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializer _serializer;

        public ErrorMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception, _serializer);
            }
        }

        public Task HandleExceptionAsync(HttpContext context, Exception exception, JsonSerializer serializer)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            using (var writer = new StreamWriter(context.Response.Body))
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var retorno = new
                {
                    sucesso = false,
                    erros = new[]
                    {
                        exception.Message,
                        exception.InnerException != null ? exception.InnerException.ToString() : exception.ToString()
                    }
                };

                serializer.Serialize(writer, retorno);
                return writer.FlushAsync();
            }
        }
    }

    public static class ErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseError(this IApplicationBuilder builder)
            => builder.UseMiddleware<ErrorMiddleware>();
    }
}
