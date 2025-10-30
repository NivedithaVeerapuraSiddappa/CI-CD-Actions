using System;
using GitHubMigrationTool.Interfaces;

namespace GitHubMigrationTool.Infrastructure
{
    /// <summary>
    /// Console-based logger implementation
    /// Follows Single Responsibility Principle - only handles console output
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.Write(message);
        }

        public void LogLine(string message = "")
        {
            Console.WriteLine(message);
        }

        public void LogError(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ {message}");
            Console.ForegroundColor = originalColor;
        }

        public void LogSuccess(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {message}");
            Console.ForegroundColor = originalColor;
        }
    }
}
