import subprocess
import os
import unittest
import time

class TestGitHubMigrationTool(unittest.TestCase):
    """Test cases for GitHub Migration Tool"""
    
    def setUp(self):
        """Set up test environment"""
        self.project_path = r"C:\CME\GitHubCopilot\Day2"
        self.dotnet_run = ["dotnet", "run", "--project", self.project_path]
    
    def test_01_test_mode_execution(self):
        """Test: Run application in test mode with dummy data"""
        print("\n[TEST 1] Running test mode...")
        result = subprocess.run(
            self.dotnet_run + ["test"],
            capture_output=True,
            text=True,
            timeout=30
        )
        
        self.assertEqual(result.returncode, 0, "Application should exit successfully")
        self.assertIn("TEST MODE", result.stdout, "Should indicate test mode")
        self.assertIn("SUCCESS", result.stdout, "Should show success status")
        self.assertIn("awesome-project", result.stdout, "Should migrate awesome-project")
        self.assertIn("demo-app", result.stdout, "Should migrate demo-app")
        self.assertIn("test-library", result.stdout, "Should migrate test-library")
        self.assertIn("Total Repositories Migrated: 3", result.stdout, "Should migrate 3 repos")
        self.assertIn("Total Issues Migrated: 10", result.stdout, "Should migrate 10 issues")
        print("[PASS] Test mode execution successful")
    
    def test_02_missing_arguments(self):
        """Test: Run without required arguments (should show usage)"""
        print("\n[TEST 2] Testing missing arguments...")
        result = subprocess.run(
            self.dotnet_run,
            capture_output=True,
            text=True,
            timeout=10
        )
        
        self.assertIn("Usage:", result.stdout, "Should display usage information")
        self.assertIn("source-owner", result.stdout, "Should mention source-owner parameter")
        self.assertIn("target-owner", result.stdout, "Should mention target-owner parameter")
        self.assertIn("personal-access-token", result.stdout, "Should mention PAT parameter")
        self.assertIn("Test mode: dotnet run test", result.stdout, "Should mention test mode")
        print("[PASS] Usage message displayed correctly")
    
    def test_03_with_command_line_args(self):
        """Test: Run with command line arguments (will fail without valid PAT, but should validate input)"""
        print("\n[TEST 3] Testing with command line arguments...")
        result = subprocess.run(
            self.dotnet_run + ["test-source", "test-target", "fake-token-123"],
            capture_output=True,
            text=True,
            timeout=30
        )
        
        # Should attempt to run (may fail on API call with fake token, but that's expected)
        self.assertIn("GitHub Migration Tool", result.stdout, "Should start the tool")
        print("[PASS] Command line arguments accepted")
    
    def test_04_environment_variables(self):
        """Test: Run with environment variables"""
        print("\n[TEST 4] Testing with environment variables...")
        
        env = os.environ.copy()
        env["SOURCE_OWNER"] = "test-source-env"
        env["TARGET_OWNER"] = "test-target-env"
        env["GITHUB_PAT"] = "fake-pat-from-env"
        
        result = subprocess.run(
            self.dotnet_run,
            capture_output=True,
            text=True,
            timeout=30,
            env=env
        )
        
        self.assertIn("GitHub Migration Tool", result.stdout, "Should start the tool with env vars")
        print("[PASS] Environment variables recognized")
    
    def test_05_test_mode_output_structure(self):
        """Test: Verify test mode output structure and formatting"""
        print("\n[TEST 5] Validating output structure...")
        result = subprocess.run(
            self.dotnet_run + ["test"],
            capture_output=True,
            text=True,
            timeout=30
        )
        
        output = result.stdout
        
        # Check for key sections
        self.assertIn("=== GitHub Migration Tool ===", output, "Should have header")
        self.assertIn("Fetching repositories", output, "Should show fetching process")
        self.assertIn("Processing repository:", output, "Should show processing")
        self.assertIn("Created repository:", output, "Should show creation")
        self.assertIn("Migrating", output, "Should show migration progress")
        self.assertIn("Migration Status:", output, "Should show final status")
        self.assertIn("Migrated Items Summary:", output, "Should show summary")
        self.assertIn("Migration process completed", output, "Should show completion")
        print("[PASS] Output structure is correct")
    
    def test_06_test_mode_performance(self):
        """Test: Verify test mode completes within reasonable time"""
        print("\n[TEST 6] Testing performance...")
        start_time = time.time()
        
        result = subprocess.run(
            self.dotnet_run + ["test"],
            capture_output=True,
            text=True,
            timeout=30
        )
        
        end_time = time.time()
        execution_time = end_time - start_time
        
        self.assertEqual(result.returncode, 0, "Should complete successfully")
        self.assertLess(execution_time, 10, "Should complete within 10 seconds")
        print(f"[PASS] Completed in {execution_time:.2f} seconds")
    
    def test_07_case_insensitive_test_mode(self):
        """Test: Verify test mode is case insensitive"""
        print("\n[TEST 7] Testing case insensitivity...")
        
        for test_arg in ["test", "TEST", "Test", "TeSt"]:
            result = subprocess.run(
                self.dotnet_run + [test_arg],
                capture_output=True,
                text=True,
                timeout=30
            )
            
            self.assertIn("TEST MODE", result.stdout, f"Should recognize '{test_arg}' as test mode")
        
        print("[PASS] Test mode is case insensitive")
    
    def test_08_build_project(self):
        """Test: Verify project builds without errors"""
        print("\n[TEST 8] Testing project build...")
        result = subprocess.run(
            ["dotnet", "build", self.project_path],
            capture_output=True,
            text=True,
            timeout=60
        )
        
        self.assertEqual(result.returncode, 0, "Project should build successfully")
        self.assertIn("Build succeeded", result.stdout, "Should show build success")
        print("[PASS] Project builds successfully")

def run_tests():
    """Run all test cases"""
    print("=" * 60)
    print("GitHub Migration Tool - Test Suite")
    print("=" * 60)
    
    # Create test suite
    loader = unittest.TestLoader()
    suite = loader.loadTestsFromTestCase(TestGitHubMigrationTool)
    
    # Run tests
    runner = unittest.TextTestRunner(verbosity=2)
    result = runner.run(suite)
    
    # Print summary
    print("\n" + "=" * 60)
    print("Test Summary")
    print("=" * 60)
    print(f"Tests run: {result.testsRun}")
    print(f"Successes: {result.testsRun - len(result.failures) - len(result.errors)}")
    print(f"Failures: {len(result.failures)}")
    print(f"Errors: {len(result.errors)}")
    print("=" * 60)
    
    return result.wasSuccessful()

if __name__ == "__main__":
    success = run_tests()
    exit(0 if success else 1)
