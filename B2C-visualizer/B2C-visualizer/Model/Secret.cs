using System.Text.Json.Serialization;

namespace B2C_visualizer.Model
{
    public class Secret
    {
        [JsonPropertyName("keyId")]
        public string Id { get; set; }
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
        [JsonPropertyName("hint")]
        public string Hint { get; set; }
        [JsonPropertyName("startDate")]
        public DateTimeOffset StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTimeOffset EndDate { get; set; }
    }
}