using Newtonsoft.Json;

namespace Circus.Shared.Models;

public class MessageListResponse
{
    [JsonProperty("object")]
    public string Object { get; set; }

    [JsonProperty("data")]
    public List<Message> Data { get; set; }
}

public class Message
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("object")]
    public string Object { get; set; }

    [JsonProperty("created_at")]
    public long CreatedAt { get; set; }

    [JsonProperty("assistant_id")]
    public string AssistantId { get; set; }

    [JsonProperty("thread_id")]
    public string ThreadId { get; set; }

    [JsonProperty("run_id")]
    public string RunId { get; set; }

    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("content")]
    public List<Content> Content { get; set; }
}

public class Content
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("text")]
    public Text Text { get; set; }
}

public class Text
{
    [JsonProperty("value")]
    public string Value { get; set; }
}