# GitHub Migration Tool - Web Application

A modern web application for migrating GitHub repositories and issues between organizations with an intuitive, beautiful user interface.

## Features

- **Test Mode**: Run a quick test migration with dummy data to verify functionality
- **Normal Mode**: Perform actual migration between GitHub organizations
- **Beautiful UI**: Modern, responsive design with animations and glassmorphism effects
- **Real-time Results**: See migration results instantly in the browser
- **Downloadable Reports**: Generate and download timestamped migration reports
- **Secure**: Uses Personal Access Tokens (PAT) for authentication

## Prerequisites

- .NET 8.0 SDK
- Valid GitHub Personal Access Token(s) with appropriate permissions
- Python 3.x (for running tests)

## Project Structure

```
Day2/
├── WebApp/
│   ├── Controllers/
│   │   └── MigrationController.cs
│   ├── Views/
│   │   └── Migration/
│   │       └── Index.cshtml
│   ├── Program.cs
│   ├── WebApp.csproj
│   └── appsettings.json
├── Services/
│   ├── GitHubService.cs
│   └── MigrationService.cs
├── Models/
│   ├── Repository.cs
│   ├── Issue.cs
│   └── MigrationStatus.cs
└── Tests/
    ├── test_webapp.py
    ├── test_migration_tool.py
    ├── test_integration.py
    ├── test_report_generation.py
    └── run_all_tests.py
```

## Getting Started

### Building the Application

```powershell
cd C:\CME\GitHubCopilot\Day2\WebApp
dotnet build
```

### Running the Application

```powershell
cd C:\CME\GitHubCopilot\Day2\WebApp
dotnet run
```

The application will start and be accessible at:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

### Accessing the Application

Open your browser and navigate to:
```
https://localhost:5001/Migration
```

## Using the Application

### Test Mode

1. Click on the **Test Mode** card
2. Click the **Run Test Migration** button
3. View the results immediately
4. Download the report if needed

Test mode uses dummy data:
- 3 test repositories
- 10 test issues
- No GitHub API calls required

### Normal Mode

1. Click on the **Normal Mode** card
2. Fill in the required fields:
   - **Source Personal Access Token**: Your GitHub PAT for the source organization
   - **Source Organization**: The GitHub organization to migrate from
   - **Target Personal Access Token**: Your GitHub PAT for the target organization
   - **Target Organization**: The GitHub organization to migrate to
3. Click **Start Migration**
4. View the migration results
5. Download the detailed report

## API Endpoints

### GET /Migration
Main page with the user interface

### POST /Migration/RunTestMode
Run a test migration with dummy data

**Response:**
```json
{
  "success": true,
  "message": "Test mode executed successfully",
  "report": "Full report text...",
  "reportPath": "C:\\path\\to\\TestReport_20240101_120000.txt",
  "timestamp": "20240101_120000"
}
```

### POST /Migration/RunNormalMode
Run an actual migration between GitHub organizations

**Request Body:**
```json
{
  "sourceToken": "ghp_xxxxxxxxxx",
  "sourceOrg": "source-organization",
  "targetToken": "ghp_yyyyyyyyyy",
  "targetOrg": "target-organization"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Migration completed successfully",
  "report": "Full report text...",
  "reportPath": "C:\\path\\to\\MigrationReport_20240101_120000.txt",
  "timestamp": "20240101_120000",
  "stats": {
    "repositoriesMigrated": 5,
    "issuesMigrated": 23
  }
}
```

### GET /Migration/DownloadReport
Download a generated report file

**Query Parameters:**
- `timestamp`: The timestamp of the report (e.g., "20240101_120000")
- `mode`: Either "test" or "normal"

## Testing

### Running All Tests

```powershell
cd C:\CME\GitHubCopilot\Day2\Tests
python run_all_tests.py
```

This will run:
- Console application tests (8 tests)
- Integration tests (5 tests)
- Report generation tests (7 tests)
- Web application tests (8 tests)

**Total: 28 tests**

### Running Web Application Tests Only

**Note:** The web application must be running before executing these tests.

```powershell
# Terminal 1: Start the web application
cd C:\CME\GitHubCopilot\Day2\WebApp
dotnet run

# Terminal 2: Run the tests
cd C:\CME\GitHubCopilot\Day2\Tests
python test_webapp.py
```

The web application tests include:
1. Application accessibility
2. Test mode endpoint
3. Test mode report content
4. Timestamp format validation
5. Normal mode parameter validation
6. Invalid credentials handling
7. JSON response structure
8. Concurrent request handling

## Report Files

Migration reports are automatically saved with timestamps:

**Test Mode:**
- Format: `TestReport_YYYYMMDD_HHMMSS.txt`
- Example: `TestReport_20240315_143022.txt`

**Normal Mode:**
- Format: `MigrationReport_YYYYMMDD_HHMMSS.txt`
- Example: `MigrationReport_20240315_143530.txt`

Reports are saved in the application's working directory.

## Security Considerations

- Personal Access Tokens are never stored or logged
- HTTPS is enforced in production environments
- Tokens are transmitted securely via HTTPS
- Report files may contain sensitive information - store them securely

## Creating a GitHub Personal Access Token

1. Go to GitHub Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click "Generate new token" (classic)
3. Give it a descriptive name
4. Select scopes:
   - `repo` (full control of private repositories)
   - `admin:org` (if migrating organization repositories)
5. Click "Generate token"
6. Copy the token immediately (you won't see it again)

## Troubleshooting

### Port Already in Use

If port 5001 is already in use, you can specify a different port:

```powershell
dotnet run --urls="https://localhost:7001;http://localhost:7000"
```

### SSL Certificate Warnings

On first run, you may need to trust the development certificate:

```powershell
dotnet dev-certs https --trust
```

### Web Application Tests Failing

Make sure:
1. The web application is running (`dotnet run`)
2. You can access https://localhost:5001/Migration in your browser
3. The `requests` Python package is installed (`pip install requests`)

## Technology Stack

- **Backend**: ASP.NET Core 8.0 (C#)
- **Frontend**: HTML5, CSS3, JavaScript (Vanilla)
- **Design**: Glassmorphism, Gradient backgrounds, Animations
- **API**: GitHub REST API v3
- **Testing**: Python unittest, requests library

## License

This project is created for educational purposes.

## Support

For issues or questions:
1. Check the test results for specific error messages
2. Review the generated report files for detailed information
3. Ensure your Personal Access Tokens have the correct permissions
4. Verify network connectivity to GitHub API

## Future Enhancements

Potential improvements:
- Database integration for migration history
- User authentication and authorization
- Scheduled migrations
- Webhook integration
- Migration rollback functionality
- Progress tracking for large migrations
- Email notifications
- API rate limit handling
- Detailed logging and audit trails
