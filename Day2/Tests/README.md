# GitHub Migration Tool - Test Suite

## Overview
This test suite provides comprehensive testing for the GitHub Migration Tool using Python's unittest framework.

## Test Files

### 1. `test_migration_tool.py`
Basic functionality tests:
- Test mode execution with dummy data
- Missing arguments handling
- Command line argument parsing
- Environment variable support
- Output structure validation
- Performance testing
- Case insensitivity
- Build verification

### 2. `test_integration.py`
Integration tests:
- Repository count accuracy
- Issue count accuracy
- Complete repository processing
- Migration workflow sequence
- Error-free execution

### 3. `run_all_tests.py`
Test runner that executes all test suites and generates a comprehensive report.

## Prerequisites

- Python 3.7 or higher
- .NET SDK (for building and running the C# application)
- The GitHub Migration Tool project built successfully

## Running Tests

### Run All Tests
```bash
cd C:\CME\GitHubCopilot\Day2\Tests
python run_all_tests.py
```

### Run Specific Test Suite
```bash
# Basic functionality tests
python test_migration_tool.py

# Integration tests
python test_integration.py
```

### Run Individual Test
```bash
python test_migration_tool.py TestGitHubMigrationTool.test_01_test_mode_execution
```

## Test Coverage

### Functional Tests
- ✓ Test mode with dummy data
- ✓ Argument validation
- ✓ Environment variable handling
- ✓ Output formatting
- ✓ Error handling

### Integration Tests
- ✓ Data accuracy (repositories and issues)
- ✓ Complete workflow execution
- ✓ Sequence validation
- ✓ Error-free operation

### Performance Tests
- ✓ Execution time limits
- ✓ Response time validation

## Expected Results

All tests should pass with the test mode:
- 3 repositories migrated
- 10 issues migrated
- Execution time < 10 seconds
- No errors in output
- Proper workflow sequence

## Troubleshooting

### Test Timeout
If tests timeout, increase the timeout value in the test files (default: 30 seconds).

### Build Errors
Ensure the project builds successfully:
```bash
dotnet build C:\CME\GitHubCopilot\Day2
```

### Path Issues
Verify the project path in test files matches your setup:
```python
self.project_path = r"C:\CME\GitHubCopilot\Day2"
```

## Adding New Tests

1. Create test methods in the appropriate test class
2. Follow naming convention: `test_XX_descriptive_name`
3. Add assertions to validate expected behavior
4. Update this README with new test descriptions

## CI/CD Integration

These tests can be integrated into GitHub Actions or other CI/CD pipelines:

```yaml
- name: Run Python Tests
  run: |
    cd Tests
    python run_all_tests.py
```

## Test Results Format

```
============================================================
GitHub Migration Tool - Complete Test Suite
Started at: 2025-10-29 15:30:45
============================================================

Running: test_migration_tool.py
[TEST 1] Running test mode...
✓ Test mode execution successful
...

============================================================
FINAL TEST SUMMARY
============================================================
test_migration_tool.py: ✓ PASSED
test_integration.py: ✓ PASSED

Test Suites: 2/2 passed
Completed at: 2025-10-29 15:31:20
============================================================
```
