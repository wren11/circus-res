using System.Text.Json.Serialization;

namespace Circus.Shared;

public class JsonStep2Output
{
    [JsonPropertyName("candidate_id")] public Guid CandidateId { get; set; }

    [JsonPropertyName("file_id")] public string FileId { get; set; }

    [JsonPropertyName("assistant_id")] public string AssistantId { get; set; }

    [JsonPropertyName("run_id")] public string RunId { get; set; }

    [JsonPropertyName("message_id")] public string MessageId { get; set; }

    [JsonPropertyName("candidate_info")] public CandidateInfo CandidateInfo { get; set; }

    [JsonPropertyName("candidate_score")] public int CandidateScore { get; set; }

    [JsonPropertyName("questions")] public List<Question> Questions { get; set; }
}