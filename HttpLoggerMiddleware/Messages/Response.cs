using Microsoft.AspNetCore.Http;

namespace HttpLoggerMiddleware.Messages
{
    /// <summary>
    /// Represent an HTTP Response
    /// </summary>
    internal sealed class Response : MessageBase
    {
        private readonly Stream _originalBody;

        /// <summary>
        /// Constructor
        /// </summary>
        internal Response(HttpResponse response) : base(response, response.Headers)
        {
            _originalBody = response.Body;
            response.Body = new MemoryStream();
        }

        /// <summary>
        /// Extracts string from stream
        /// </summary>
        private async Task<string> ExtractStrFromStream(Stream stream)
        {
            var buffer = new MemoryStream();

            await stream.CopyToAsync(buffer);
            buffer.Position = 0;

            var str = await new StreamReader(buffer)
                .ReadToEndAsync();

            buffer.Position = 0;
            stream = buffer;

            return str;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected async override Task<dynamic> GetPayload()
        {
            var body = ((HttpResponse)_message).Body;
            body.Position = 0;

            var payload = await ExtractStrFromStream(body);

            body.Position = 0;

            await body.CopyToAsync(_originalBody);

            ((HttpResponse)_message).Body = _originalBody;

            return payload.ToJson();
        }
    }
}
