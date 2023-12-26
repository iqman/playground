using System.Text.Json.Serialization;

namespace B2C_visualizer.Model
{
    public class CallBackUrl
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}