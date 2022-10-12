using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware: IMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
             IHostEnvironment env )
        {
            _logger = logger;
            _next = next;
            _env = env;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
              await _next(context);
            }
            catch(Exception ex)
            {
              _logger.LogError(ex, ex.Message);
              context.Response.ContentType = "application/json";
              context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

              var response = _env.IsDevelopment()
                  ? new APIException(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
                  : new APIException(context.Response.StatusCode, "Internal server error");

              var options = new JsonSerializerOptions{
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
              };

              var json = JsonSerializer.Serialize(response,options);
              await context.Response.WriteAsync(json);
            }
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}