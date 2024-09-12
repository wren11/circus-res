using System.Text.Json.Serialization;

namespace Circus.Shared;

public class Education
{
    [JsonPropertyName("degree")]
    public string Degree { get; set; }

    [JsonPropertyName("institution")]
    public string Institution { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("honors")]
    public string Honors { get; set; }
}