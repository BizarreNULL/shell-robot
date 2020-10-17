using System.Text.Json.Serialization;

namespace ShellRobot.Shared.Models
{
    public class Expression
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("expressions")]
        public string[] Regexes { get; set; }
    }
}