using System.Threading.Tasks;

namespace GitHubMigrationTool.Interfaces
{
    /// <summary>
    /// Interface for generating migration reports
    /// </summary>
    public interface IReportGenerator
    {
        void AddLine(string message = "");
        Task<string> GenerateReportAsync(string reportType);
        string GetReportContent();
    }
}
