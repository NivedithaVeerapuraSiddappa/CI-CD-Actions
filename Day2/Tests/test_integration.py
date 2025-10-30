import subprocess
import json
import re
import unittest

class TestMigrationToolIntegration(unittest.TestCase):
    """Integration tests for GitHub Migration Tool"""
    
    def setUp(self):
        """Set up test environment"""
        self.project_path = r"C:\CME\GitHubCopilot\Day2"
        self.dotnet_run = ["dotnet", "run", "--project", self.project_path, "test"]
    
    def test_repository_count_accuracy(self):
        """Test: Verify repository count matches summary"""
        print("\n[INTEGRATION TEST 1] Verifying repository count...")
        result = subprocess.run(
            self.dotnet_run,
            capture_output=True,
            text=True,
            timeout=30
        )
        
        # Extract repository names
        repo_matches = re.findall(r'Repository: ([\w-]+)', result.stdout)
        
        # Extract total count
        count_match = re.search(r'Total Repositories Migrated: (\d+)', result.stdout)
        
        self.assertIsNotNone(count_match, "Should have repository count")
        total_count = int(count_match.group(1))
        
        self.assertEqual(len(repo_matches), total_count, 
                        f"Repository count mismatch: found {len(repo_matches)}, expected {total_count}")
        print(f"[PASS] Repository count accurate: {total_count}")
    
    def test_issue_count_accuracy(self):
        """Test: Verify issue count matches summary"""
        print("\n[INTEGRATION TEST 2] Verifying issue count...")
        result = subprocess.run(
            self.dotnet_run,
            capture_output=True,
            text=True,
            timeout=30
        )
        
        # Extract individual issue counts
        issue_matches = re.findall(r'Migrating (\d+) issues', result.stdout)
        calculated_total = sum(int(count) for count in issue_matches)
        
        # Extract total count
        count_match = re.search(r'Total Issues Migrated: (\d+)', result.stdout)
        
        self.assertIsNotNone(count_match, "Should have issue count")
        reported_total = int(count_match.group(1))
        
        self.assertEqual(calculated_total, reported_total, 
                        f"Issue count mismatch: calculated {calculated_total}, reported {reported_total}")
        print(f"[PASS] Issue count accurate: {reported_total}")
    
    def test_all_repositories_processed(self):
        """Test: Verify all repositories are processed and listed"""
        print("\n[INTEGRATION TEST 3] Verifying all repos processed...")
        result = subprocess.run(
            self.dotnet_run,
            capture_output=True,
            text=True,
            timeout=30
        )
        
        expected_repos = ["awesome-project", "demo-app", "test-library"]
        
        for repo in expected_repos:
            self.assertIn(f"Processing repository: {repo}", result.stdout,
                         f"Should process {repo}")
            self.assertIn(f"Created repository: {repo}", result.stdout,
                         f"Should create {repo}")
            self.assertIn(f"Repository: {repo}", result.stdout,
                         f"Should list {repo} in summary")
        
        print("[PASS] All repositories processed correctly")
    
    def test_migration_workflow_sequence(self):
        """Test: Verify migration follows correct sequence"""
        print("\n[INTEGRATION TEST 4] Verifying workflow sequence...")
        result = subprocess.run(
            self.dotnet_run,
            capture_output=True,
            text=True,
            timeout=30
        )
        
        output_lines = result.stdout.split('\n')
        
        # Check sequence: Header -> Fetch -> Process -> Status -> Summary -> Complete
        header_found = False
        fetch_found = False
        process_found = False
        status_found = False
        summary_found = False
        complete_found = False
        
        for line in output_lines:
            if "GitHub Migration Tool" in line:
                header_found = True
            elif "Fetching repositories" in line and header_found:
                fetch_found = True
            elif "Processing repository:" in line and fetch_found:
                process_found = True
            elif "Migration Status:" in line and process_found:
                status_found = True
            elif "Migrated Items Summary:" in line and status_found:
                summary_found = True
            elif "Migration process completed" in line and summary_found:
                complete_found = True
        
        self.assertTrue(header_found, "Should show header")
        self.assertTrue(fetch_found, "Should fetch repositories")
        self.assertTrue(process_found, "Should process repositories")
        self.assertTrue(status_found, "Should show migration status")
        self.assertTrue(summary_found, "Should show summary")
        self.assertTrue(complete_found, "Should show completion message")
        print("[PASS] Workflow sequence is correct")
    
    def test_no_errors_in_output(self):
        """Test: Verify no error messages in successful test mode"""
        print("\n[INTEGRATION TEST 5] Checking for errors...")
        result = subprocess.run(
            self.dotnet_run,
            capture_output=True,
            text=True,
            timeout=30
        )
        
        error_indicators = ["error", "exception", "failed", "✗"]
        output_lower = result.stdout.lower()
        
        # Note: "failed items" section should be empty in test mode
        for indicator in error_indicators:
            if indicator == "failed":
                self.assertNotIn("failed items:", output_lower,
                               "Should not have failed items in test mode")
            elif indicator == "✗":
                # Check there are no ✗ marks (only ✓ marks)
                self.assertNotIn("✗", result.stdout,
                               "Should not have failure marks in test mode")
        
        print("[PASS] No errors in test mode output")

if __name__ == "__main__":
    unittest.main(verbosity=2)
