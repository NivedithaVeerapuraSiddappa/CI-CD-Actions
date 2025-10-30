import subprocess
import os
import glob
import re
import unittest
from datetime import datetime, timedelta

class TestReportGeneration(unittest.TestCase):
    """Test cases for test report generation"""
    
    def setUp(self):
        """Set up test environment"""
        self.project_path = r"C:\CME\GitHubCopilot\Day2"
        self.dotnet_run = ["dotnet", "run", "--project", self.project_path]
        
        # Clean up old test reports before running
        self.cleanup_old_reports()
    
    def cleanup_old_reports(self):
        """Remove old test report files"""
        report_pattern = os.path.join(self.project_path, "TestReport_*.txt")
        for report_file in glob.glob(report_pattern):
            try:
                os.remove(report_file)
            except Exception as e:
                print(f"Warning: Could not remove {report_file}: {e}")
    
    def test_01_report_file_created(self):
        """Test: Verify report file is created with timestamp"""
        print("\n[TEST 1] Checking if report file is created...")
        
        # Run test mode
        result = subprocess.run(
            self.dotnet_run + ["test"],
            capture_output=True,
            text=True,
            timeout=30,
            cwd=self.project_path
        )
        
        # Find generated report files
        report_pattern = os.path.join(self.project_path, "TestReport_*.txt")
        report_files = glob.glob(report_pattern)
        
        self.assertGreater(len(report_files), 0, "Report file should be created")
        self.assertIn("Test report generated:", result.stdout, 
                     "Should show report generation message")
        
        print(f"[PASS] Report file created: {os.path.basename(report_files[0])}")
    
    def test_02_report_filename_format(self):
        """Test: Verify report filename follows correct format"""
        print("\n[TEST 2] Validating report filename format...")
        
        result = subprocess.run(
            self.dotnet_run + ["test"],
            capture_output=True,
            text=True,
            timeout=30,
            cwd=self.project_path
        )
        
        report_pattern = os.path.join(self.project_path, "TestReport_*.txt")
        report_files = glob.glob(report_pattern)
        
        self.assertGreater(len(report_files), 0, "Report file should exist")
        
        # Check filename format: TestReport_YYYYMMDD_HHMMSS.txt
        filename = os.path.basename(report_files[0])
        pattern = r"TestReport_\d{8}_\d{6}\.txt"
        
        self.assertRegex(filename, pattern, 
                        f"Filename should match pattern TestReport_YYYYMMDD_HHMMSS.txt")
        
        print(f"[PASS] Filename format is correct: {filename}")
    
    def test_03_report_content_completeness(self):
        """Test: Verify report contains all expected sections"""
        print("\n[TEST 3] Checking report content completeness...")
        
        result = subprocess.run(
            self.dotnet_run + ["test"],
            capture_output=True,
            text=True,
            timeout=30,
            cwd=self.project_path
        )
        
        report_pattern = os.path.join(self.project_path, "TestReport_*.txt")
        report_files = glob.glob(report_pattern)
        
        self.assertGreater(len(report_files), 0, "Report file should exist")
        
        # Read report content
        with open(report_files[0], 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Check for required sections
        required_sections = [
            "GitHub Migration Tool - TEST MODE",
            "Test Run:",
            "Report File:",
            "Source Owner: test-source-user",
            "Target Owner: test-target-user",
            "Fetching repositories",
            "Processing repository:",
            "Migration Status:",
            "Migrated Items Summary:",
            "Total Repositories Migrated:",
            "Total Issues Migrated:",
            "Test completed at:",
            "Migration process completed"
        ]
        
        for section in required_sections:
            self.assertIn(section, content, f"Report should contain: {section}")
        
        print("[PASS] Report contains all required sections")
    
    def test_04_report_repository_details(self):
        """Test: Verify report contains correct repository details"""
        print("\n[TEST 4] Validating repository details in report...")
        
        result = subprocess.run(
            self.dotnet_run + ["test"],
            capture_output=True,
            text=True,
            timeout=30,
            cwd=self.project_path
        )
        
        report_pattern = os.path.join(self.project_path, "TestReport_*.txt")
        report_files = glob.glob(report_pattern)
        
        with open(report_files[0], 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Check for all three test repositories
        expected_repos = ["awesome-project", "demo-app", "test-library"]
        for repo in expected_repos:
            self.assertIn(f"Processing repository: {repo}", content)
            self.assertIn(f"Created repository: {repo}", content)
            self.assertIn(f"Repository: {repo}", content)
        
        # Check for correct counts
        self.assertIn("Total Repositories Migrated: 3", content)
        self.assertIn("Total Issues Migrated: 10", content)
        
        print("[PASS] Repository details are correct in report")
    
    def test_05_report_timestamp_accuracy(self):
        """Test: Verify report timestamps are reasonable"""
        print("\n[TEST 5] Checking report timestamp accuracy...")
        
        start_time = datetime.now()
        
        result = subprocess.run(
            self.dotnet_run + ["test"],
            capture_output=True,
            text=True,
            timeout=30,
            cwd=self.project_path
        )
        
        end_time = datetime.now()
        
        report_pattern = os.path.join(self.project_path, "TestReport_*.txt")
        report_files = glob.glob(report_pattern)
        
        with open(report_files[0], 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Extract timestamp from filename
        filename = os.path.basename(report_files[0])
        timestamp_match = re.search(r"TestReport_(\d{8})_(\d{6})\.txt", filename)
        
        self.assertIsNotNone(timestamp_match, "Should extract timestamp from filename")
        
        date_str = timestamp_match.group(1)
        time_str = timestamp_match.group(2)
        
        # Parse timestamp
        file_timestamp = datetime.strptime(f"{date_str}_{time_str}", "%Y%m%d_%H%M%S")
        
        # Verify timestamp is within test execution window (allow 2 second buffer)
        self.assertGreaterEqual(file_timestamp, start_time.replace(microsecond=0) - 
                               timedelta(seconds=2))
        self.assertLessEqual(file_timestamp, end_time.replace(microsecond=0) + 
                           timedelta(seconds=2))
        
        print(f"[PASS] Timestamp is accurate: {file_timestamp}")
    
    def test_06_report_matches_console_output(self):
        """Test: Verify report content matches console output"""
        print("\n[TEST 6] Comparing report with console output...")
        
        result = subprocess.run(
            self.dotnet_run + ["test"],
            capture_output=True,
            text=True,
            timeout=30,
            cwd=self.project_path
        )
        
        report_pattern = os.path.join(self.project_path, "TestReport_*.txt")
        report_files = glob.glob(report_pattern)
        
        with open(report_files[0], 'r', encoding='utf-8') as f:
            report_content = f.read()
        
        console_output = result.stdout
        
        # Key phrases that should be in both
        key_phrases = [
            "awesome-project",
            "demo-app",
            "test-library",
            "Total Repositories Migrated: 3",
            "Total Issues Migrated: 10",
            "Migration Status:",
            "SUCCESS"
        ]
        
        for phrase in key_phrases:
            self.assertIn(phrase, console_output, 
                         f"Console should contain: {phrase}")
            self.assertIn(phrase, report_content, 
                         f"Report should contain: {phrase}")
        
        print("[PASS] Report content matches console output")
    
    def test_07_multiple_reports_unique_names(self):
        """Test: Verify multiple test runs create unique report files"""
        print("\n[TEST 7] Testing unique report names for multiple runs...")
        
        report_files_before = set(glob.glob(
            os.path.join(self.project_path, "TestReport_*.txt")))
        
        # Run test twice
        for i in range(2):
            subprocess.run(
                self.dotnet_run + ["test"],
                capture_output=True,
                text=True,
                timeout=30,
                cwd=self.project_path
            )
            # Small delay to ensure different timestamps
            import time
            time.sleep(1)
        
        report_files_after = set(glob.glob(
            os.path.join(self.project_path, "TestReport_*.txt")))
        
        new_reports = report_files_after - report_files_before
        
        self.assertEqual(len(new_reports), 2, 
                        "Should create 2 unique report files")
        
        # Verify filenames are different
        filenames = [os.path.basename(f) for f in new_reports]
        self.assertEqual(len(filenames), len(set(filenames)), 
                        "Report filenames should be unique")
        
        print(f"[PASS] Created {len(new_reports)} unique report files")
    
    def tearDown(self):
        """Clean up after tests - keep last report for inspection"""
        pass

if __name__ == "__main__":
    unittest.main(verbosity=2)
