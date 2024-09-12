using Circus.Shared.Models;
using Microsoft.AspNetCore.Http;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Circus.Services.Abstraction;

public interface IHttpClientService
{
    Task<string> GetAssistantMessageAsync(string threadId);
    Task<string> AddUserMessageAsync(string assistantId, string threadId, string userMessage);
    Task<string> GenerateFollowUpInstructions(string promptFile);
    Task<string> ListMessagesAsync(string threadId);
    Task<string> CreateRunAsync(string threadId, string assistantId);
    Task<string> CreateVectorStoreAsync(string name, string[] fileIds);
    Task AddFileToVectorStoreAsync(string vectorStoreId, string fileId);
    Task<UploadFileResponse> UploadFileToOpenAi(Guid uploadId, IFormFile file, IProgress<UploadProgress> progress);
    Task<string> CreateThreadAsync();
    Task AttachMessageWithAttachmentAsync(string threadId, string messageContent, string fileId);
    Task<CreateAssistantResponse> CreateAssistant(string vectorStoreId, string promptFile);
}