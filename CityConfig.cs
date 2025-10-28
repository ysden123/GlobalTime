using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlobalTime
{
    /// <summary>
    /// Represents the configuration settings for a city, including its name and timezone.
    /// </summary>
    /// <remarks>This class is used to store and manage basic configuration details for a city. It provides
    /// properties to access the city's name and timezone, and can be serialized to JSON.</remarks>
    class CityConfig
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("timezone")]
        public string? TimeZone { get; set; }

        public override string ToString()
        {
            return $"CityConfig: {JsonSerializer.Serialize<CityConfig>(this)}";
        }
    }
}
