using System.Text.Json.Serialization;

namespace Circus.Shared;

public class Involvement
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("organization")]
    public string Organization { get; set; }

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; }

    [JsonPropertyName("details")]
    public string Details { get; set; }
}