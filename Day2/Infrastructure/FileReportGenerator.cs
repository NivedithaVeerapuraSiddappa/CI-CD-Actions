using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GitHubMigrationTool.Interfaces;

namespace GitHubMigrationTool.Infrastructure
{
    /// <summary>
    /// File-based report generator implementation
    /// Follows Single Responsibility Principle - only handles report generation and file I/O
    /// Follows Open/Closed Principle - can be extended without modification
    /// </summary>
    public class FileReportGenerator : IReportGenerator
    {
        private readonly StringBuilder _reportContent;

        public FileReportGenerator()
        {
            _reportContent = new StringBuilder();
        }

        public void AddLine(string message = "")
        {
            _reportContent.AppendLine(message);
        }

        public async Task<string> GenerateReportAsync(string reportType)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var reportFileName = $"{reportType}Report_{timestamp}.txt";
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), reportFileName);

            await File.WriteAllTextAsync(reportPath, _reportContent.ToString());
            
            return reportPath;
        }

        public string GetReportContent()
        {
            return _reportContent.ToString();
        }
    }
}
