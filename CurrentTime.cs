using System.Text.Json.Serialization;

namespace GlobalTime
{
    /// <summary>
    /// Response from server
    /// </summary>
    /// <remarks>This record is used to encapsulate time-related data, which can be serialized to or
    /// deserialized from JSON.</remarks>
    internal record CurrentTime
    {
        [JsonPropertyName("timezone")]
        public string TimeZone { get; set; } = string.Empty;
        [JsonPropertyName("utc_offset")]
        public int UTCOffset { get; set; } = 0;
        [JsonPropertyName("local_time")]
        public string LocalTime { get; set; } = string.Empty;
    }
}
