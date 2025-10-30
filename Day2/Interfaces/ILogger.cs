namespace GitHubMigrationTool.Interfaces
{
    /// <summary>
    /// Interface for logging messages to various outputs
    /// </summary>
    public interface ILogger
    {
        void Log(string message);
        void LogLine(string message = "");
        void LogError(string message);
        void LogSuccess(string message);
    }
}
