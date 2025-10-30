using System.Threading.Tasks;

namespace GitHubMigrationTool.Interfaces
{
    /// <summary>
    /// Interface for orchestrating migration operations
    /// </summary>
    public interface IMigrationOrchestrator
    {
        Task ExecuteAsync();
    }
}
