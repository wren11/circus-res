using System.Text.Json.Serialization;

namespace Circus.Shared;

public class Answer
{
    [JsonPropertyName("answer_text")]
    public string AnswerText { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }
}