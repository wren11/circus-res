# Circus

## Overview

Circus is a sophisticated system designed to analyze resumes, generate job-specific and skill-specific questions, evaluate answers, and provide a comprehensive score. Leveraging the power of OpenAI and generative AI, Circus automates the process of resume analysis and candidate evaluation, ensuring a thorough and unbiased assessment.

## Features

### Resume Analysis
- **Detailed Extraction**: Extracts comprehensive information from resumes, including candidate details, skills, education, experience, involvement, and projects.
- **JSON Schema Compliance**: Ensures all extracted data fits predefined JSON schemas for consistency and accuracy.

### Question Generation
- **Custom Questions**: Generates technical, behavioral, problem-solving, and code-test questions tailored to the candidate's resume.
- **Randomization**: Ensures questions are unique and randomly generated for each candidate.
- **Difficulty Levels**: Categorizes questions by difficulty (easy, medium, hard, complex) to match the candidate's experience level.

### Answer Evaluation
- **Automated Scoring**: Evaluates answers and assigns scores based on predefined criteria.
- **Fact-Checking**: Ensures all questions and answers are accurate and fact-checked.
- **Feedback**: Provides detailed feedback on answers, including correct answers and areas for improvement.

### API Capabilities
- **File Upload**: Allows users to upload resumes for analysis.
- **Status Check**: Provides endpoints to check the status of the analysis and question generation process.
- **Progress Reporting**: Reports the progress of file uploads and analysis in real-time.

## How It Works

### OpenAI Integration
Circus integrates with OpenAI to leverage its generative AI capabilities. The system uses OpenAI's models to:
- Analyze resumes and extract relevant information.
- Generate custom questions based on the extracted data.
- Evaluate answers and provide detailed feedback.

### API Endpoints

#### Upload File
Endpoint: `POST /openai/upload-file`
- **Description**: Uploads a resume file for analysis.
- **Parameters**: `IFormFile file`, `CancellationToken cancellationToken`
- **Response**: Returns the upload ID and status.

#### Check Status
Endpoint: `GET /openai/check-status/{runId}/{threadId}`
- **Description**: Checks the status of the analysis and question generation process.
- **Parameters**: `string threadId`, `string runId`
- **Response**: Returns the current status and any generated results.

### Detailed Code References


## Getting Started

### Prerequisites
- .NET 8.0 SDK
- OpenAI API Key

### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/your-repo/circus.git
   ```
2. Navigate to the project directory:
   ```sh
   cd circus
   ```
3. Restore dependencies:
   ```sh
   dotnet restore
   ```

### Configuration
1. Update the `appsettings.json` file with your OpenAI API key and other necessary configurations.

### Running the Application
1. Build the project:
   ```sh
   dotnet build
   ```
2. Run the application:
   ```sh
   dotnet run
   ```

### Testing
The project includes comprehensive unit tests using xUnit. To run the tests:
