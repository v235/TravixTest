using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FM.Web.ExceptionHandler
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError($"{DateTime.Now} - Error:{context.Exception.Message}" +
                             $" - StackTrace:{context.Exception.StackTrace}");

            context.ExceptionHandled = true;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int) HttpStatusCode.InternalServerError; 
            response.ContentType = "application/json";
            var err = "Server error";
            response.WriteAsync(err);
        }
    }
}
