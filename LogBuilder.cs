using Serilog;
using System.IO;

namespace GlobalTime
{
    /// <summary>
    /// Provides functionality to initialize logging for an application.
    /// </summary>
    /// <remarks>This static class configures a logger using the Serilog library. It sets up logging to both
    /// the console and a file, with different logging levels for debug and release builds. The logger is initialized
    /// only once per application run.</remarks>
    static class LogBuilder
    {
        private static bool _isInitialized = false;

        public static void Initialize(string assemblyName, string logFilePrefix)
        {
            if (!_isInitialized)
            {
                var folder = YSCommon.Utils.GetAssemblyFolderInLocalData(assemblyName);
                string fileName = Path.Combine(folder, "logs", $"{logFilePrefix}.log");
#if DEBUG
                Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Debug()
                   .Enrich.WithThreadId()
                   .WriteTo.File(fileName,
                   rollingInterval: RollingInterval.Month,
                   outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext} [{ThreadId}] {Message:lj}{NewLine}{Exception}")
               .CreateLogger();
#else
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .Enrich.WithThreadId()
               .WriteTo.File(fileName,
               rollingInterval: RollingInterval.Month,
               outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext} [{ThreadId}] {Message:lj}{NewLine}{Exception}")
           .CreateLogger();
#endif
                _isInitialized = true;
            }
        }
    }
}
