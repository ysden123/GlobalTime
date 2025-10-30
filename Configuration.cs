using Serilog;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlobalTime
{
    /// <summary>
    /// Represents the configuration settings for the application, including API keys and city-specific configurations.
    /// </summary>
    /// <remarks>This class is used to deserialize configuration data from a JSON file located in the user's
    /// application data directory. It includes settings such as the API key and a list of city
    /// configurations.</remarks>
    internal class Configuration
    {
        private static ILogger _logger = Log.ForContext<Configuration>();
        [JsonPropertyName("apiKey")]
        public string? ApiKey { get; set; }

        [JsonPropertyName("cities")]
        public List<CityConfig>? Cities { get; set; }

        internal static Configuration? ReadConfiguration()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YSGlobalTime", "configuration.json");
            try
            {
                Configuration? configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(path));
                _logger.Debug("Configuration read from {Path}: {Configuration}", path, configuration != null ? configuration : "null");
                return configuration;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error reading configuration from {Path}", path);
                return null;
            }
        }

        public override string ToString()
        {
            return $"Configuration: {JsonSerializer.Serialize<Configuration>(this)}";
        }
    }
}
