using System;
using System.Threading.Tasks;
using GitHubMigrationTool.Interfaces;
using GitHubMigrationTool.Services;

namespace GitHubMigrationTool.Orchestrators
{
    /// <summary>
    /// Orchestrates real GitHub migration
    /// Follows Single Responsibility Principle - only handles real migration logic
    /// Follows Dependency Inversion Principle - depends on abstractions
    /// </summary>
    public class RealMigrationOrchestrator : IMigrationOrchestrator
    {
        private readonly ILogger _logger;
        private readonly IReportGenerator _reportGenerator;
        private readonly GitHubService _gitHubService;
        private readonly string _sourceOwner;
        private readonly string _targetOwner;

        public RealMigrationOrchestrator(
            ILogger logger, 
            IReportGenerator reportGenerator,
            GitHubService gitHubService,
            string sourceOwner,
            string targetOwner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reportGenerator = reportGenerator ?? throw new ArgumentNullException(nameof(reportGenerator));
            _gitHubService = gitHubService ?? throw new ArgumentNullException(nameof(gitHubService));
            _sourceOwner = sourceOwner ?? throw new ArgumentNullException(nameof(sourceOwner));
            _targetOwner = targetOwner ?? throw new ArgumentNullException(nameof(targetOwner));
        }

        public async Task ExecuteAsync()
        {
            LogAndRecord("=== GitHub Migration Tool ===");
            LogAndRecord($"Migration Run: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            LogAndRecord();
            LogAndRecord($"Source Owner: {_sourceOwner}");
            LogAndRecord($"Target Owner: {_targetOwner}");
            LogAndRecord();

            var migrationService = new MigrationService(_gitHubService, _sourceOwner, _targetOwner);
            var migrationStatus = await migrationService.ExecuteMigrationAsync();

            LogAndRecord();
            LogAndRecord("==================================================");
            
            if (migrationStatus.Success)
            {
                _logger.LogSuccess("Migration Status: SUCCESS");
                _reportGenerator.AddLine("Migration Status: SUCCESS");
            }
            else
            {
                _logger.LogError("Migration Status: FAILED");
                _reportGenerator.AddLine("Migration Status: FAILED");
            }
            
            LogAndRecord("==================================================");
            LogAndRecord($"Message: {migrationStatus.Message}");
            LogAndRecord();
            
            if (migrationStatus.MigratedItems.Count > 0)
            {
                LogAndRecord("Migrated Items Summary:");
                foreach (var item in migrationStatus.MigratedItems)
                {
                    LogAndRecord($"  - {item}");
                }
                LogAndRecord();
            }

            if (migrationStatus.FailedItems.Count > 0)
            {
                LogAndRecord("Failed Items:");
                foreach (var item in migrationStatus.FailedItems)
                {
                    LogAndRecord($"  - {item}");
                }
                LogAndRecord();
            }

            LogAndRecord($"Total Repositories Migrated: {migrationStatus.RepositoriesMigrated}");
            LogAndRecord($"Total Issues Migrated: {migrationStatus.IssuesMigrated}");
            
            // Generate report
            var reportPath = await _reportGenerator.GenerateReportAsync("Migration");
            
            _logger.LogLine();
            _logger.LogSuccess($"Migration report generated: {reportPath}");
            _logger.LogLine("Migration process completed.");
        }

        private void LogAndRecord(string message = "")
        {
            _logger.LogLine(message);
            _reportGenerator.AddLine(message);
        }
    }
}
