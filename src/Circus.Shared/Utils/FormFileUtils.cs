using Microsoft.AspNetCore.Http;

namespace Circus.Shared.Utils;

public static class FormFileUtils
{
    /// <summary>
    /// Creates an IFormFile instance from the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the file to be converted.</param>
    /// <returns>An IFormFile instance representing the file.</returns>
    public static IFormFile CreateFormFile(string filePath)
    {
        // Get the file name
        var fileName = Path.GetFileName(filePath);

        // Open the file as a FileStream
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        // Create a form file
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream" // Set the content type as needed
        };

        return formFile;
    }
}