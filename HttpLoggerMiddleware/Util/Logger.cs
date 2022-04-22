#nullable disable
using Newtonsoft.Json;

namespace HttpLoggerMiddleware.Util
{
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

        private static List<dynamic> Logs { get; set; }

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

                Logs = JsonConvert.
                    DeserializeObject<List<dynamic>>(json);
            }
            catch
            {
                Logs = new List<dynamic>();
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        internal Task Log(dynamic item)
        {
            return Task.Run(() =>
            {
                Logs.Add(item);

                _lock.EnterWriteLock();

                try
                {
                    var json = JsonConvert
                        .SerializeObject(Logs, Formatting.Indented);

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
