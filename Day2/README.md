# GitHub Migration Tool

A .NET console application to migrate GitHub repositories and issues from one account to another using Personal Access Token (PAT) authentication.

## Features

- Migrate repositories from source to target GitHub account
- Copy repository metadata (name, description, privacy settings)
- Migrate issues from source repositories to target repositories
- Real-time migration status and progress updates
- Detailed summary of migrated and failed items
- No external dependencies beyond .NET HTTP client

## Prerequisites

- .NET 8.0 SDK or later
- GitHub Personal Access Token with appropriate permissions:
  - `repo` (full control of private repositories)
  - `public_repo` (access to public repositories)

## Setup

1. Clone this repository
2. Navigate to the project directory
3. Restore dependencies:
   ```bash
   dotnet restore
   ```

## Usage

### Option 1: Command Line Arguments
```bash
dotnet run <source-owner> <target-owner> <personal-access-token>
```

### Option 2: Environment Variables
Set the following environment variables:
- `SOURCE_OWNER`: GitHub username/organization to migrate from
- `TARGET_OWNER`: GitHub username/organization to migrate to
- `GITHUB_PAT`: Your GitHub Personal Access Token

Then run:
```bash
dotnet run
```

### Example
```bash
dotnet run octocat myaccount ghp_xxxxxxxxxxxxxxxxxxxx
```

## Project Structure

```
GitHubMigrationTool/
├── Program.cs                    # Entry point
├── Services/
│   ├── GitHubService.cs         # GitHub API interactions
│   └── MigrationService.cs      # Migration logic
├── Models/
│   ├── Repository.cs            # Repository model
│   ├── Issue.cs                 # Issue model
│   └── MigrationStatus.cs       # Migration status model
└── GitHubMigrationTool.csproj   # Project file
```

## How It Works

1. **Authenticate**: Uses Personal Access Token for GitHub API authentication
2. **Fetch Repositories**: Retrieves all repositories from the source owner
3. **Create Repositories**: Creates corresponding repositories in the target account
4. **Migrate Issues**: Copies issues from source to target repositories
5. **Report Status**: Provides detailed summary of migration results

## Limitations

- Does not migrate Git history (commits, branches, tags)
- Does not migrate pull requests (only issues)
- Does not migrate repository collaborators or webhooks
- Rate limited by GitHub API (5000 requests/hour for authenticated users)

## Security Notes

- Never commit your Personal Access Token to version control
- Store tokens securely using environment variables or secret management systems
- Tokens have full access to your account - handle with care

## Error Handling

The tool includes comprehensive error handling for:
- Network failures
- API rate limiting
- Invalid credentials
- Repository name conflicts
- Missing permissions

## License

MIT License - feel free to modify and distribute as needed.
