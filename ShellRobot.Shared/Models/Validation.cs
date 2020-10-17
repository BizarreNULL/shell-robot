using System.Text.Json.Serialization;

namespace ShellRobot.Shared.Models
{
    public class Validation
    {
        [JsonPropertyName("anyMatch")]
        public bool AnyMatch { get; set; }

        [JsonPropertyName("expressions")]
        public Expression[] Expressions { get; set; }
    }
}