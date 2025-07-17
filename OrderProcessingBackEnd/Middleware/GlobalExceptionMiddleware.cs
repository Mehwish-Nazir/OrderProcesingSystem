using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
namespace OrderProcessingBackEnd.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        } 

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception occur");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                //use object type for usig both (development , production env), for one env can use var instaed of using object
                object response = _env.IsDevelopment()
                    ? new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "An unexpected error occurred.",
                        Detailed = ex.Message
                    }
                    : new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "An unexpected error occurred. Please try again later."
                        //detailed=Detailed = ex.Message
                        //can't use detailed message here in production to avoid exposing data
                    };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
                //register in program.cs file 
                //app.UseMiddleware<GlobalExceptionMiddleware>();

            }
        }
    }
}
