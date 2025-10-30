using System.Collections.Generic;

namespace GitHubMigrationTool.Models
{
    public class MigrationStatus
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> MigratedItems { get; set; } = new List<string>();
        public List<string> FailedItems { get; set; } = new List<string>();
        public int RepositoriesMigrated { get; set; }
        public int IssuesMigrated { get; set; }
    }
}
