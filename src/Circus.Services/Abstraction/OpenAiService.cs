using Circus.Services.Hubs;
using Circus.Shared;
using Circus.Shared.Models;
using Circus.Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Circus.Services.Abstraction
{
    public class OpenAiService(
        IHttpClientService httpClientService,
        ILogger<OpenAiService> logger,
        IHubContext<UploadProgressHub> hubContext)
            : IOpenAiService
    {


        public async Task<RunResponse<JsonStep1Output>> CheckStatus(string threadId, string runId)
        {
            var runStatus = new RunResponse<JsonStep1Output>();

            try
            {
                var statusResponse = await httpClientService.ListMessagesAsync(threadId);
                runStatus = JsonConvert.DeserializeObject<RunResponse<JsonStep1Output>>(statusResponse);

                var message = await httpClientService.GetAssistantMessageAsync(threadId);
                var extractionResult = JsonParser.ParseCandidateRequest(message);
                runStatus!.Content = extractionResult;

                runStatus.Status = "completed";
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while checking status");
                runStatus!.Status = "in-progress";
            }

            return runStatus;
        }


        public async Task<UploadFileResponseDto> UploadFile(Guid uploadId, IFormFile file, IProgress<UploadProgress> progress)
        {
            if (file == null || file.Length == 0)
            {
                logger.LogWarning("No file uploaded.");
                throw new ArgumentException("No file uploaded.");
            }

            try
            {
                var uploadResponse = await httpClientService.UploadFileToOpenAi(uploadId, file, progress);
                var vectorStoreId = await httpClientService.CreateVectorStoreAsync("test_vec", [uploadResponse.Id]);

                // Attach resources
                var resourcesPath = Path.Combine(AppContext.BaseDirectory, "Resources");
                var files = Directory.EnumerateFiles(resourcesPath);

                foreach (var resourceFile in files)
                {
                    var result = await httpClientService.UploadFileToOpenAi(uploadId, FormFileUtils.CreateFormFile(resourceFile), progress);
                    await httpClientService.AddFileToVectorStoreAsync(vectorStoreId, result.Id);
                }

                await httpClientService.AddFileToVectorStoreAsync(vectorStoreId, uploadResponse.Id);

                var assistantResponse = await httpClientService.CreateAssistant(vectorStoreId, "ExtractPrompt.txt");
                var threadId = await httpClientService.CreateThreadAsync();

                await httpClientService.AttachMessageWithAttachmentAsync(threadId, "Process and generate randomized questions for this resume and provide me json response.", uploadResponse.Id);
                var run = await httpClientService.CreateRunAsync(threadId, assistantResponse.Id);

                return new UploadFileResponseDto
                {
                    Run = run,
                    ThreadId = threadId,
                    AssistantId = assistantResponse.Id
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the file upload");
                throw;
            }
        }

        public async Task ReportProgress(Guid uploadId, string fileName, double percentage)
        {
            await hubContext.Clients.All.SendAsync("UploadProgress", new UploadProgress
            {
                UploadId = uploadId,
                FileName = fileName,
                Percentage = percentage
            });
        }
    }

    public interface IOpenAiService
    {
        Task<RunResponse<JsonStep1Output>> CheckStatus(string threadId, string runId);
        Task<UploadFileResponseDto> UploadFile(Guid uploadId, IFormFile file, IProgress<UploadProgress> progress);
        Task ReportProgress(Guid uploadProgressUploadId, string uploadProgressFileName, double uploadProgressPercentage);
    }
}
