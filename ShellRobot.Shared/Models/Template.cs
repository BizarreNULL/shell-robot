using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShellRobot.Shared.Models
{
    public class Template
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("args")]
        public Dictionary<string, ArgsOption> Args { get; set; }

        [JsonPropertyName("template")]
        public string Content { get; set; }
    }
}