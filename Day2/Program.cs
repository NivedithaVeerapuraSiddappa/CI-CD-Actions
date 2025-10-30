using System;
using System.Threading.Tasks;
using GitHubMigrationTool.Infrastructure;
using GitHubMigrationTool.Orchestrators;
using GitHubMigrationTool.Services;

namespace GitHubMigrationTool
{
    /// <summary>
    /// Main entry point - follows SOLID principles
    /// S - Single Responsibility: Only handles program initialization and routing
    /// O - Open/Closed: Can add new orchestrators without modifying this class
    /// D - Dependency Inversion: Depends on abstractions, creates concrete implementations
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== GitHub Migration Tool ===\n");

            // Check for test mode
            bool testMode = args.Length > 0 && args[0].ToLower() == "test";

            if (testMode)
            {
                Console.WriteLine("Running in TEST MODE with dummy data...\n");
                await RunTestModeAsync();
                return;
            }

            // Read configuration from environment variables or command line arguments
            string? sourceOwner = Environment.GetEnvironmentVariable("SOURCE_OWNER") ?? (args.Length > 0 ? args[0] : null);
            string? targetOwner = Environment.GetEnvironmentVariable("TARGET_OWNER") ?? (args.Length > 1 ? args[1] : null);
            string? personalAccessToken = Environment.GetEnvironmentVariable("GITHUB_PAT") ?? (args.Length > 2 ? args[2] : null);

            if (string.IsNullOrEmpty(sourceOwner) || string.IsNullOrEmpty(targetOwner) || string.IsNullOrEmpty(personalAccessToken))
            {
                Console.WriteLine("Usage: dotnet run <source-owner> <target-owner> <personal-access-token>");
                Console.WriteLine("Or set environment variables: SOURCE_OWNER, TARGET_OWNER, GITHUB_PAT");
                Console.WriteLine("\nTest mode: dotnet run test");
                return;
            }

            await RunRealMigrationAsync(sourceOwner, targetOwner, personalAccessToken);
        }

        /// <summary>
        /// Factory method for test mode - creates and configures test orchestrator
        /// Follows Dependency Injection pattern
        /// </summary>
        static async Task RunTestModeAsync()
        {
            var logger = new ConsoleLogger();
            var reportGenerator = new FileReportGenerator();
            var orchestrator = new TestMigrationOrchestrator(logger, reportGenerator);
            
            await orchestrator.ExecuteAsync();
        }

        /// <summary>
        /// Factory method for real migration - creates and configures real orchestrator
        /// Follows Dependency Injection pattern
        /// </summary>
        static async Task RunRealMigrationAsync(string sourceOwner, string targetOwner, string personalAccessToken)
        {
            var logger = new ConsoleLogger();
            var reportGenerator = new FileReportGenerator();
            
            using var gitHubService = new GitHubService(personalAccessToken);
            var orchestrator = new RealMigrationOrchestrator(
                logger, 
                reportGenerator, 
                gitHubService, 
                sourceOwner, 
                targetOwner);
            
            await orchestrator.ExecuteAsync();
        }
    }
}
