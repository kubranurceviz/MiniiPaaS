// MiniiPaaS.API/Middleware/LoggingMiddleware.cs
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MiniiPaaS.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                await _next(context);
                sw.Stop();

                var statusCode = context.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                Log.ForContext("RequestMethod", context.Request.Method)
                   .ForContext("RequestPath", context.Request.Path)
                   .ForContext("StatusCode", statusCode)
                   .ForContext("Elapsed", sw.ElapsedMilliseconds)
                   .Write(level, "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed} ms");
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log.ForContext("RequestMethod", context.Request.Method)
                   .ForContext("RequestPath", context.Request.Path)
                   .ForContext("Exception", ex)
                   .Error(ex, "HTTP {RequestMethod} {RequestPath} threw an exception after {Elapsed} ms",
                          context.Request.Method, context.Request.Path, sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}