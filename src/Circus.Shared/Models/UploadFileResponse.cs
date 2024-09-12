namespace Circus.Shared.Models;

public class UploadFileResponse
{
    public string Object { get; set; }
    public string Id { get; set; }
    public string Purpose { get; set; }
    public string Filename { get; set; }
    public long Bytes { get; set; }
    public int CreatedAt { get; set; }
    public string Status { get; set; }
    public object StatusDetails { get; set; }
}