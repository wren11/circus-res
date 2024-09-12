using System.Text.Json.Serialization;


namespace Circus.Shared;

// Candidate Information Model
public class CandidateInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("skills")]
        public Skills Skills { get; set; }

        [JsonPropertyName("education")]
        public Education Education { get; set; }

        [JsonPropertyName("certifications")]
        public List<string> Certifications { get; set; }

        [JsonPropertyName("experience")]
        public List<Experience> Experience { get; set; }

        [JsonPropertyName("involvement")]
        public List<Involvement> Involvement { get; set; }

        [JsonPropertyName("projects")]
        public List<Project> Projects { get; set; }
    }

// Question Model

// JSON Step 1 Output Model

// JSON Step 2 Input/Output Model