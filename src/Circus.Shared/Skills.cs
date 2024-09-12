using System.Text.Json.Serialization;

namespace Circus.Shared;

public class Skills
{
    [JsonPropertyName("Programming Languages")]
    public List<string> ProgrammingLanguages { get; set; }

    [JsonPropertyName("Frameworks/Platforms")]
    public List<string> FrameworksPlatforms { get; set; }

    [JsonPropertyName("Technologies/Concepts")]
    public List<string> TechnologiesConcepts { get; set; }

    [JsonPropertyName("Database")]
    public List<string> Database { get; set; }

    [JsonPropertyName("Cloud Platforms")]
    public List<string> CloudPlatforms { get; set; }

    [JsonPropertyName("IoT/Connectivity")]
    public List<string> IoTConnectivity { get; set; }

    [JsonPropertyName("AI/ML")]
    public List<string> AIML { get; set; }

    [JsonPropertyName("Security")]
    public List<string> Security { get; set; }

    [JsonPropertyName("Other")]
    public List<string> Other { get; set; }
}