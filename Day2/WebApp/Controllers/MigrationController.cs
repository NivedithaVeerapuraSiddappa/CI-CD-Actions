using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GitHubMigrationTool.Services;
using GitHubMigrationTool.Models;

namespace WebApp.Controllers
{
    public class MigrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RunTestMode()
        {
            try
            {
                var report = new StringBuilder();
                report.AppendLine("GitHub Migration Tool - Test Mode Report");
                report.AppendLine($"Generated at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine(new string('=', 60));
                report.AppendLine();

                // Simulate test data
                report.AppendLine("Test Repositories (3):");
                report.AppendLine("  1. test-repo-1 - A test repository");
                report.AppendLine("  2. test-repo-2 - Another test repository");
                report.AppendLine("  3. test-repo-3 - Third test repository");
                report.AppendLine();

                report.AppendLine("Test Issues (10):");
                for (int i = 1; i <= 10; i++)
                {
                    report.AppendLine($"  Issue #{i}: Test issue {i}");
                }
                report.AppendLine();

                report.AppendLine("Migration Status:");
                report.AppendLine("  Repositories Migrated: 3");
                report.AppendLine("  Issues Migrated: 10");
                report.AppendLine("  Status: SUCCESS");
                report.AppendLine();
                report.AppendLine("Test mode completed successfully!");

                // Save report with timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string reportPath = Path.Combine(Directory.GetCurrentDirectory(), $"TestReport_{timestamp}.txt");
                await System.IO.File.WriteAllTextAsync(reportPath, report.ToString());

                return Json(new
                {
                    success = true,
                    message = "Test mode executed successfully",
                    report = report.ToString(),
                    reportPath = reportPath,
                    timestamp = timestamp
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RunNormalMode(string sourceToken, string sourceOrg, string targetToken, string targetOrg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceToken) || string.IsNullOrWhiteSpace(sourceOrg) ||
                    string.IsNullOrWhiteSpace(targetToken) || string.IsNullOrWhiteSpace(targetOrg))
                {
                    return Json(new
                    {
                        success = false,
                        message = "All parameters are required"
                    });
                }

                var report = new StringBuilder();
                report.AppendLine("GitHub Migration Tool - Normal Mode Report");
                report.AppendLine($"Generated at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine(new string('=', 60));
                report.AppendLine();

                var gitHubService = new GitHubService(sourceToken);
                var migrationService = new MigrationService(gitHubService, sourceOrg, targetOrg);

                var result = await migrationService.ExecuteMigrationAsync();

                report.AppendLine($"Source Organization: {sourceOrg}");
                report.AppendLine($"Target Organization: {targetOrg}");
                report.AppendLine();
                report.AppendLine("Migration Results:");
                report.AppendLine($"  Repositories Migrated: {result.RepositoriesMigrated}");
                report.AppendLine($"  Issues Migrated: {result.IssuesMigrated}");
                report.AppendLine($"  Status: {(result.Success ? "SUCCESS" : "FAILED")}");
                
                if (!result.Success)
                {
                    report.AppendLine($"  Error: {result.Message}");
                }

                if (result.MigratedItems.Count > 0)
                {
                    report.AppendLine();
                    report.AppendLine("Migrated Items:");
                    foreach (var item in result.MigratedItems)
                    {
                        report.AppendLine($"  - {item}");
                    }
                }

                if (result.FailedItems.Count > 0)
                {
                    report.AppendLine();
                    report.AppendLine("Failed Items:");
                    foreach (var item in result.FailedItems)
                    {
                        report.AppendLine($"  - {item}");
                    }
                }

                // Save report with timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string reportPath = Path.Combine(Directory.GetCurrentDirectory(), $"MigrationReport_{timestamp}.txt");
                await System.IO.File.WriteAllTextAsync(reportPath, report.ToString());

                return Json(new
                {
                    success = result.Success,
                    message = result.Message,
                    report = report.ToString(),
                    reportPath = reportPath,
                    timestamp = timestamp,
                    stats = new
                    {
                        repositoriesMigrated = result.RepositoriesMigrated,
                        issuesMigrated = result.IssuesMigrated
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpGet]
        public IActionResult DownloadReport(string timestamp, string mode)
        {
            try
            {
                string fileName = mode == "test" ? $"TestReport_{timestamp}.txt" : $"MigrationReport_{timestamp}.txt";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("Report file not found");
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "text/plain", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading report: {ex.Message}");
            }
        }
    }
}
