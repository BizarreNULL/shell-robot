using System.Text.Json.Serialization;

namespace ShellRobot.Shared.Models
{
    public class ArgsOption
    {
        [JsonPropertyName("mandatory")]
        public bool Mandatory { get; set; }

        [JsonPropertyName("pattern")]
        public string Pattern { get; set; }

        [JsonPropertyName("validations")]
        public Validation Validation { get; set; }
    }
}