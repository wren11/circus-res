using System.Text.Json.Serialization;

namespace Circus.Shared;

public class Question
{
    [JsonPropertyName("question_id")]
    public string QuestionId { get; set; }

    [JsonPropertyName("question_text")]
    public string QuestionText { get; set; }

    [JsonPropertyName("question_type")]
    public string QuestionType { get; set; }

    [JsonPropertyName("difficulty")]
    public string Difficulty { get; set; }

    [JsonPropertyName("time_limit")]
    public int TimeLimit { get; set; }

    [JsonPropertyName("code_scope")]
    public string CodeScope { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("options")]
    public List<string> Options { get; set; }

    [JsonPropertyName("answers")]
    public List<Answer> Answers { get; set; } = new List<Answer>();
}