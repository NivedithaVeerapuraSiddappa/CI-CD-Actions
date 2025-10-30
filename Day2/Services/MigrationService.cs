using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitHubMigrationTool.Models;

namespace GitHubMigrationTool.Services
{
    public class MigrationService
    {
        private readonly GitHubService _gitHubService;
        private readonly string _sourceOwner;
        private readonly string _targetOwner;

        public MigrationService(GitHubService gitHubService, string sourceOwner, string targetOwner)
        {
            _gitHubService = gitHubService;
            _sourceOwner = sourceOwner;
            _targetOwner = targetOwner;
        }

        public async Task<MigrationStatus> ExecuteMigrationAsync()
        {
            var status = new MigrationStatus
            {
                Success = false,
                Message = "Migration started..."
            };

            try
            {
                Console.WriteLine($"Fetching repositories from source owner: {_sourceOwner}");
                var sourceRepos = await _gitHubService.GetRepositoriesAsync(_sourceOwner);

                if (sourceRepos.Count == 0)
                {
                    status.Message = "No repositories found to migrate.";
                    status.Success = true;
                    return status;
                }

                Console.WriteLine($"Found {sourceRepos.Count} repositories to migrate.\n");

                foreach (var repo in sourceRepos)
                {
                    Console.WriteLine($"Processing repository: {repo.Name}");
                    
                    // Create repository in target account
                    bool repoCreated = await _gitHubService.CreateRepositoryAsync(
                        repo.Name, 
                        repo.Description, 
                        repo.Private
                    );

                    if (repoCreated)
                    {
                        status.MigratedItems.Add($"Repository: {repo.Name}");
                        status.RepositoriesMigrated++;

                        // Migrate issues if repository was created successfully
                        Console.WriteLine($"  Fetching issues for {repo.Name}...");
                        var issues = await _gitHubService.GetIssuesAsync(_sourceOwner, repo.Name);

                        if (issues.Count > 0)
                        {
                            Console.WriteLine($"  Migrating {issues.Count} issues...");
                            foreach (var issue in issues)
                            {
                                // Skip pull requests (they appear as issues in GitHub API)
                                if (issue.PullRequest != null)
                                    continue;

                                bool issueCreated = await _gitHubService.CreateIssueAsync(
                                    _targetOwner,
                                    repo.Name,
                                    issue.Title,
                                    issue.Body,
                                    issue.Labels
                                );

                                if (issueCreated)
                                {
                                    status.IssuesMigrated++;
                                }
                            }
                            status.MigratedItems.Add($"  └─ {status.IssuesMigrated} issues from {repo.Name}");
                        }
                    }
                    else
                    {
                        status.FailedItems.Add($"Repository: {repo.Name}");
                    }

                    Console.WriteLine(); // Empty line for readability
                }

                status.Success = true;
                status.Message = $"Migration completed successfully. {status.RepositoriesMigrated} repositories and {status.IssuesMigrated} issues migrated.";
            }
            catch (Exception ex)
            {
                status.Success = false;
                status.Message = $"Migration failed with error: {ex.Message}";
            }

            return status;
        }
    }
}
