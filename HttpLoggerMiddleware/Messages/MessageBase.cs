using Microsoft.AspNetCore.Http;

namespace HttpLoggerMiddleware.Messages
{
    /// <summary>
    /// Abstract base class that represent an HTTP Message
    /// </summary>
    internal abstract class MessageBase
    {
        protected readonly object _message;
        public readonly IHeaderDictionary _headers;

        /// <summary>
        /// Constructor
        /// </summary>
        protected MessageBase(object message, IHeaderDictionary headers)
        {
            _message = message;
            _headers = headers;
        }

        /// <summary>
        /// Extracts the payload from the HTTP message;
        /// </summary>
        protected abstract Task<dynamic> GetPayload();

        /// <summary>
        /// Extracts the dictionary of the headers from the HTTP message
        /// </summary>
        private dynamic GetHeaders()
        {
            return _headers.ToDict().ToJson();
        }

        /// <summary>
        /// Gets the log item of the current message (headers & payload)
        /// </summary>
        internal async Task<dynamic> GetLogItemAsync()
        {
            return new
            {
                Headers = GetHeaders(),
                Body = await GetPayload(),
            };
        }
    }
}
