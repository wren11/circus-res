using Circus.Services.Abstraction;
using Circus.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Circus.Controllers;

[ApiController]
[Route("[controller]")]
public class OpenAiController(IOpenAiService openAiService, ILogger<OpenAiController> logger) : ControllerBase
{
    [HttpGet("check-status/{runId}/{threadId}")]
    public async Task<IActionResult> CheckStatus(string threadId, string runId)
    {
        try
        {
            var runStatus = await openAiService.CheckStatus(threadId, runId);

            if (runStatus.Status == "completed")
            {

            }

            return Ok(runStatus);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while checking status");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("upload-file")]
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
    {
        var uploadId = Guid.NewGuid();
        IProgress<UploadProgress> progress = new Progress<UploadProgress>(uploadProgress =>
        {
            openAiService.ReportProgress(uploadProgress.UploadId, uploadProgress.FileName, uploadProgress.Percentage);
        });

        try
        {
            var upload = await openAiService.UploadFile(uploadId, file, progress);
            return Ok(upload);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while queuing the file upload");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}