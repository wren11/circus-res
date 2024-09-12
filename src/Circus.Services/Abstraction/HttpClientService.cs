using Circus.Shared.Models;
using Circus.Shared.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenAI;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using OpenAI.Assistants;

#pragma warning disable OPENAI001

namespace Circus.Services.Abstraction;

public class HttpClientService(HttpClient client, IOptions<OpenAiSettings> openAiSettings) : IHttpClientService
{
    private readonly string _apiKey = openAiSettings.Value.AccessToken;

    public void CreateClient()
    {
        OpenAIClient openAiClient = new OpenAIClient(new ApiKeyCredential(_apiKey), new OpenAIClientOptions());


    }


    public async Task<string> GetAssistantMessageAsync(string threadId)
    {
        var messagesResponse = await ListMessagesAsync(threadId);
        var messagesList = JsonConvert.DeserializeObject<MessageListResponse>(messagesResponse);

        var assistantMessage = messagesList?.Data.LastOrDefault(m => m.Role == "assistant");
        return assistantMessage?.Content[0].Text.Value!;
    }

    public async Task<string> AddUserMessageAsync(string assistantId, string threadId, string userMessage)
    {
        var newMessage = new
        {
            role = "user",
            content = userMessage
        };

        var jsonMessage = JsonConvert.SerializeObject(newMessage);
        var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"https://api.openai.com/v1/threads/{threadId}/messages", content);
        response.EnsureSuccessStatusCode();

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await CreateRunAsync(threadId, assistantId);
        }

        throw new Exception("Failed to add user message");
    }

    public async Task<string> GenerateFollowUpInstructions(string promptFile)
    {
        var instructionsFilePath = Path.Combine(AppContext.BaseDirectory, "Prompts", promptFile);
        var instructions = await File.ReadAllTextAsync(instructionsFilePath);

        // Escape special characters in instructions
        return instructions.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
    }

    public async Task<string> ListMessagesAsync(string threadId)
    {
        var response = await client.GetAsync($"https://api.openai.com/v1/threads/{threadId}/messages");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> CreateRunAsync(string threadId, string assistantId)
    {
        var requestBody = new { assistant_id = assistantId };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"https://api.openai.com/v1/threads/{threadId}/runs", content);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(responseBody)!;
        return result.id;
    }

    public async Task<string> CreateVectorStoreAsync(string name, string[] fileIds)
    {
        var requestBody = new
        {
            name,
            file_ids = fileIds
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api.openai.com/v1/vector_stores", content);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(responseBody)!;
        return result.id;
    }

    public async Task AddFileToVectorStoreAsync(string vectorStoreId, string fileId)
    {
        var requestBody = new { file_id = fileId };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"https://api.openai.com/v1/vector_stores/{vectorStoreId}/files", content);

        response.EnsureSuccessStatusCode();
    }

    public async Task<UploadFileResponse> UploadFileToOpenAi(Guid uploadId, IFormFile file, IProgress<UploadProgress> progress)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(file.OpenReadStream());

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        content.Add(fileContent, "file", file.FileName);
        content.Add(new StringContent("assistants"), "purpose");

        // Retain existing headers
        var originalHeaders = client.DefaultRequestHeaders.ToList();

        // Clear existing headers
        client.DefaultRequestHeaders.Clear();

        // Add custom headers
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        using var progressHandler =
                new ProgressMessageHandler(new HttpClientHandler(), progress, uploadId, file.FileName);

        using var httpClientWithProgress = new HttpClient(progressHandler);
        httpClientWithProgress.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);


        try
        {
            var response = await httpClientWithProgress.PostAsync("https://api.openai.com/v1/files", content);
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<UploadFileResponse>())!;
        }
        finally
        {
            // Restore original headers
            client.DefaultRequestHeaders.Clear();
            foreach (var header in originalHeaders)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }

    public async Task<string> CreateThreadAsync()
    {
        var requestBody = new { };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api.openai.com/v1/threads", content);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(responseBody)!;
        return result.id;
    }

    public async Task AttachMessageWithAttachmentAsync(string threadId, string messageContent, string fileId)
    {
        var messageRequestBody = new
        {
            role = "user",
            content = messageContent,
            attachments = new[]
            {
                new
                {
                    file_id = fileId,
                    tools = new[]
                    {
                        new { type = "file_search" }
                    }
                }
            }
        };

        var requestContent = new StringContent(JsonConvert.SerializeObject(messageRequestBody), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"https://api.openai.com/v1/threads/{threadId}/messages", requestContent);

        response.EnsureSuccessStatusCode();
    }

    public async Task<CreateAssistantResponse> CreateAssistant(string vectorStoreId, string promptFile)
    {
        var instructionsFilePath = Path.Combine(AppContext.BaseDirectory, "Prompts", promptFile);
        var instructions = await File.ReadAllTextAsync(instructionsFilePath);

        // Escape special characters in instructions
        instructions = instructions.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");

        string jsonPayload = $@"
        {{
            ""model"": ""gpt-4o"",
            ""tools"": [{{""type"": ""file_search""}}],
            ""name"": ""RESUME"",
            ""instructions"": ""{instructions.Replace("\"", "\\\\\"")}"",
            ""tool_resources"": {{
                ""file_search"": {{
                    ""vector_store_ids"": [""{vectorStoreId}""]
                }}
            }}
        }}";

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api.openai.com/v1/assistants", content);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CreateAssistantResponse>(responseBody)!;
    }
}