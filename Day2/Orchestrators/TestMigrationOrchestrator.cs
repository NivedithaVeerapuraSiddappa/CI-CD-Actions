using System;
using System.Threading;
using System.Threading.Tasks;
using GitHubMigrationTool.Interfaces;

namespace GitHubMigrationTool.Orchestrators
{
    /// <summary>
    /// Orchestrates test migration with dummy data
    /// Follows Single Responsibility Principle - only handles test migration logic
    /// Follows Dependency Inversion Principle - depends on abstractions (ILogger, IReportGenerator)
    /// </summary>
    public class TestMigrationOrchestrator : IMigrationOrchestrator
    {
        private readonly ILogger _logger;
        private readonly IReportGenerator _reportGenerator;

        public TestMigrationOrchestrator(ILogger logger, IReportGenerator reportGenerator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reportGenerator = reportGenerator ?? throw new ArgumentNullException(nameof(reportGenerator));
        }

        public async Task ExecuteAsync()
        {
            LogAndRecord("=== GitHub Migration Tool - TEST MODE ===");
            LogAndRecord($"Test Run: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            LogAndRecord();
            
            LogAndRecord("Source Owner: test-source-user");
            LogAndRecord("Target Owner: test-target-user");
            LogAndRecord();
            LogAndRecord("==================================================");

            // Simulate fetching repositories
            LogAndRecord("Fetching repositories from source owner: test-source-user");
            LogAndRecord("Found 3 repositories to migrate.");
            LogAndRecord();

            // Simulate repository 1
            await ProcessTestRepositoryAsync("awesome-project", 5);
            
            // Simulate repository 2
            await ProcessTestRepositoryAsync("demo-app", 3);
            
            // Simulate repository 3
            await ProcessTestRepositoryAsync("test-library", 2);

            // Summary
            LogAndRecord();
            LogAndRecord("==================================================");
            LogAndRecord("Migration Summary:");
            LogAndRecord("  Repositories Migrated: 3");
            LogAndRecord("  Issues Migrated: 10");
            LogAndRecord("  Status: SUCCESS");
            LogAndRecord("==================================================");

            // Generate report
            var reportPath = await _reportGenerator.GenerateReportAsync("Test");
            
            _logger.LogLine();
            _logger.LogSuccess($"Test report generated: {reportPath}");
            _logger.LogLine();
        }

        private async Task ProcessTestRepositoryAsync(string repoName, int issueCount)
        {
            LogAndRecord($"Processing repository: {repoName}");
            await Task.Delay(500); // Simulate API call
            
            LogAndRecord($"Created repository: {repoName}");
            LogAndRecord($"  Fetching issues for {repoName}...");
            LogAndRecord($"  Migrating {issueCount} issues...");
            
            await Task.Delay(500); // Simulate API call
            LogAndRecord();
        }

        private void LogAndRecord(string message = "")
        {
            _logger.LogLine(message);
            _reportGenerator.AddLine(message);
        }
    }
}
