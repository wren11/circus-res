using Circus.Shared;
using System.Text.Json;

namespace Circus.Services;

public class JsonParser
{
    public static JsonStep1Output ParseCandidateRequest(string jsonString)
    {
        var data = JsonCleaner.CleanAndValidateJson(jsonString);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<JsonStep1Output>(data, options)!;
    }



}