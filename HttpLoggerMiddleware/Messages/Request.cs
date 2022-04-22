using Microsoft.AspNetCore.Http;

namespace HttpLoggerMiddleware.Messages
{
    /// <summary>
    /// Represents an HTTP Request
    /// </summary>
    internal sealed class Request : MessageBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal Request(HttpRequest request) : base(request, request.Headers)
        {
            request.EnableBuffering();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task<dynamic> GetPayload()
        {
            var body = ((HttpRequest)_message).Body;

            body.Position = 0;

            var payload = await new StreamReader(body)
                .ReadToEndAsync();

            body.Position = 0;

            return payload.ToJson();
        }
    }
}
