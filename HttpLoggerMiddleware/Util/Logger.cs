#nullable disable
using Newtonsoft.Json;

namespace HttpLoggerMiddleware.Util
{
    /// <summary>
    /// Default thread-safe logger of the Middleware
    /// Responsible for writing the log.json file
    /// </summary>
    internal class Logger
    {
        private static readonly string _logDirPath = Path.Combine(
            path1: AppDomain.CurrentDomain.BaseDirectory,
            path2: "HttpLogs"
        );

        private static readonly string _logFilePath = Path.Combine(
            path1: _logDirPath,
            path2: "log.json"
        );

        private static List<dynamic> _logs;

        protected readonly ReaderWriterLockSlim _lock =
            new ReaderWriterLockSlim();

        /// <summary>
        /// Constructor
        /// </summary>
        internal Logger()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the Log directory and loads the logs into a static container
        /// </summary>
        private void Initialize()
        {
            if (!Directory.Exists(_logDirPath))
                Directory.CreateDirectory(_logDirPath);

            try
            {
                var json = File.ReadAllText(_logFilePath);

                _logs = JsonConvert.
                    DeserializeObject<List<dynamic>>(json);
            }
            catch
            {
                _logs = new List<dynamic>();
            }
        }

        /// <summary>
        /// Records the request-response pair into the log.json file
        /// </summary>
        internal Task Log(dynamic item)
        {
            return Task.Run(() =>
            {
                _logs.Add(item);

                _lock.EnterWriteLock();

                try
                {
                    var json = JsonConvert
                        .SerializeObject(_logs, Formatting.Indented);

                    File.WriteAllText(_logFilePath, json);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            });
        }
    }
}
