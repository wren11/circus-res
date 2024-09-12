namespace Circus.Shared.Models;

public class Metadata
{
}

public class UploadProgress
{
    public Guid UploadId { get; set; }
    public string FileName { get; set; }
    public double Percentage { get; set; }
}
