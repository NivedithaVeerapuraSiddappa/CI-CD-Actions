"""
Test Runner for GitHub Migration Tool
Runs all test suites and generates a comprehensive report
"""

import subprocess
import sys
import os
from datetime import datetime

def run_test_file(test_file):
    """Run a single test file and return results"""
    print(f"\n{'='*60}")
    print(f"Running: {test_file}")
    print(f"{'='*60}")
    
    result = subprocess.run(
        [sys.executable, test_file],
        capture_output=True,
        text=True
    )
    
    print(result.stdout)
    if result.stderr:
        print("STDERR:", result.stderr)
    
    return result.returncode == 0

def main():
    """Main test runner"""
    print("\n" + "="*60)
    print("GitHub Migration Tool - Complete Test Suite")
    print(f"Started at: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    print("="*60)
    
    # Get test directory
    test_dir = os.path.dirname(os.path.abspath(__file__))
    
    # Test files to run
    test_files = [
        os.path.join(test_dir, "test_migration_tool.py"),
        os.path.join(test_dir, "test_integration.py"),
        os.path.join(test_dir, "test_report_generation.py"),
        os.path.join(test_dir, "test_webapp.py")
    ]
    
    results = {}
    
    # Run each test file
    for test_file in test_files:
        if os.path.exists(test_file):
            test_name = os.path.basename(test_file)
            results[test_name] = run_test_file(test_file)
        else:
            print(f"\nWarning: Test file not found: {test_file}")
            results[os.path.basename(test_file)] = False
    
    # Print final summary
    print("\n" + "="*60)
    print("FINAL TEST SUMMARY")
    print("="*60)
    
    total_suites = len(results)
    passed_suites = sum(1 for success in results.values() if success)
    
    for test_name, success in results.items():
        status = "[PASS]" if success else "[FAIL]"
        print(f"{test_name}: {status}")
    
    print(f"\nTest Suites: {passed_suites}/{total_suites} passed")
    print(f"Completed at: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    print("="*60)
    
    # Exit with appropriate code
    sys.exit(0 if all(results.values()) else 1)

if __name__ == "__main__":
    main()
