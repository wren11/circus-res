using System.Text.Json.Serialization;

namespace Circus.Shared;

public class Project
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("technologies")]
    public List<string> Technologies { get; set; }
}