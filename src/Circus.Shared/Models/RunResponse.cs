namespace Circus.Shared.Models;

public class RunResponse<T> where T : class
{
    public string Status { get; set; } = "in-progress";
    public T Content { get; set; } = null!;
}