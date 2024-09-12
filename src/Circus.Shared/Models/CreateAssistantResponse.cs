namespace Circus.Shared.Models;

public class CreateAssistantResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public int CreatedAt { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Model { get; set; }
    public string Instructions { get; set; }
    public List<Tool> Tools { get; set; }
    public ToolResources ToolResources { get; set; }
    public Metadata Metadata { get; set; }
    public float TopP { get; set; }
    public float Temperature { get; set; }
    public string ResponseFormat { get; set; }
}