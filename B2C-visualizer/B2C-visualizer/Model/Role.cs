using System.Text.Json.Serialization;

namespace B2C_visualizer.Model
{
    public class Role
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }
        [JsonPropertyName("type")]

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoleType Type { get; set; }
    }
}


