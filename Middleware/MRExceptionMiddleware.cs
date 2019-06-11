using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MRApiCommon.Exception;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace MRApiCommon.Middleware
{
    public class MRExceptionMiddleware
    {
        protected RequestDelegate _next;
        protected ILogger _logger;

        public MRExceptionMiddleware(RequestDelegate requestDelegate, ILoggerFactory loggerFactory)
        {
            _next = requestDelegate;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception exception)
            {
                object responseObject;
                if(exception is IMRException)
                {
                    IMRException genericException = (IMRException)exception;
                    _logger.LogError($"Middleware exception:\n" +
                        $"- MR Exception handled\n" +
                        $"- JSON:\n" +
                        $"- {JsonConvert.SerializeObject(genericException.ToDictionary())}");
                    responseObject = genericException.ToDictionaryShort();
                }
                else
                {
                    responseObject = new {
                        code = "-1",
                        message = "Undefined error"
                    };
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                string content = JsonConvert.SerializeObject(responseObject);
                await context.Response.WriteAsync(content, System.Text.Encoding.UTF8);
            }
        }
    }
}
