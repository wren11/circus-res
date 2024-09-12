using System.Formats.Asn1;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Circus.Services;

public class JsonCleaner
{
    public static string CleanAndValidateJson(string input)
    {
        // Remove the ```json from the start and ``` from the end of the string
        var cleanedJson = Regex.Replace(input, @"^```json|```$", string.Empty, RegexOptions.Multiline).Trim();

        try
        {
            // Parse the JSON to ensure it is valid
            var parsedJson = JToken.Parse(cleanedJson);
            // If parsing succeeds, return the formatted JSON
            return parsedJson.ToString(Formatting.Indented);
        }
        catch (JsonReaderException)
        {
            // If parsing fails, attempt to correct common issues
            cleanedJson = CorrectJsonString(cleanedJson);

            // Try parsing again after correction
            try
            {
                var parsedJson = JToken.Parse(cleanedJson);
                return parsedJson.ToString(Formatting.Indented);
            }
            catch (JsonReaderException ex)
            {
                throw new InvalidOperationException("The provided string could not be parsed into a valid JSON format.", ex);
            }
        }
    }

    private static string CorrectJsonString(string json)
    {
        // Attempt to correct common issues
        // Here we just demonstrate a basic correction by ensuring proper braces
        json = json.Trim();

        if (!json.StartsWith("{"))
        {
            json = "{" + json;
        }

        if (!json.EndsWith("}"))
        {
            json = json + "}";
        }

        return json;
    }
}