#nullable disable
using HttpLoggerMiddleware.Util;
using Microsoft.AspNetCore.Http;
using HttpLoggerMiddleware.Messages;

namespace HttpLoggerMiddleware
{
    /// <summary>
    /// Middleware that is responsible for logging (into file) all the incoming requests
    /// and outgoing responses
    /// (HEAD, BODY)
    /// </summary>
    internal class HttpLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger _logger = new Logger();

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var req = new Request(request);
            var resp = new Response(context.Response);

            await _next(context);

            await _logger.Log(new
            {
                TimeStamp = DateTime.Now,
                Route = request.Path.Value,
                Method = request.Method,
                Parameters = request.Query.ToDict().ToJson(),
                Request = await req.GetLogItemAsync(),
                Response = await resp.GetLogItemAsync()
            });
        }
    }
}
