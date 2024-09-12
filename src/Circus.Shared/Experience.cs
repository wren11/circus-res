using System.Text.Json.Serialization;

namespace Circus.Shared;

public class Experience
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("company")]
    public string Company { get; set; }

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; }

    [JsonPropertyName("responsibilities")]
    public string Responsibilities { get; set; }
}