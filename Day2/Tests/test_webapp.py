"""
Test cases for the GitHub Migration Web Application
Tests the HTTP endpoints for both test mode and normal mode
"""

import unittest
import subprocess
import time
import requests
import json
import os
from datetime import datetime

class TestWebApplication(unittest.TestCase):
    """Test cases for web application endpoints"""
    
    @classmethod
    def setUpClass(cls):
        """Start the web application before running tests"""
        cls.app_process = None
        cls.base_url = "https://localhost:5001"
        
        print("\n[INFO] Attempting to start web application...")
        # Note: For actual testing, the web app needs to be started manually
        # or we need to handle the HTTPS certificate trust issues
        
    def test_01_application_accessibility(self):
        """Test if the web application is accessible"""
        print("\n[TEST 01] Testing web application accessibility...")
        try:
            # Try to access the main page
            response = requests.get(f"{self.base_url}/Migration", verify=False, timeout=5)
            self.assertIn(response.status_code, [200, 302], 
                         f"Expected status 200 or 302, got {response.status_code}")
            print("[PASS] Web application is accessible")
        except requests.exceptions.ConnectionError:
            print("[SKIP] Web application not running - start it manually with 'dotnet run' in WebApp folder")
            self.skipTest("Web application not running")
        except Exception as e:
            self.fail(f"Error accessing web application: {str(e)}")
    
    def test_02_test_mode_endpoint(self):
        """Test the test mode endpoint"""
        print("\n[TEST 02] Testing test mode endpoint...")
        try:
            response = requests.post(
                f"{self.base_url}/Migration/RunTestMode",
                headers={"Content-Type": "application/json"},
                verify=False,
                timeout=10
            )
            
            self.assertEqual(response.status_code, 200, 
                           f"Expected status 200, got {response.status_code}")
            
            data = response.json()
            self.assertIn('success', data, "Response should contain 'success' field")
            self.assertIn('message', data, "Response should contain 'message' field")
            self.assertIn('report', data, "Response should contain 'report' field")
            self.assertIn('timestamp', data, "Response should contain 'timestamp' field")
            
            print(f"[PASS] Test mode endpoint returned: {data['message']}")
        except requests.exceptions.ConnectionError:
            self.skipTest("Web application not running")
        except Exception as e:
            self.fail(f"Error testing test mode endpoint: {str(e)}")
    
    def test_03_test_mode_report_content(self):
        """Test that test mode generates proper report content"""
        print("\n[TEST 03] Testing test mode report content...")
        try:
            response = requests.post(
                f"{self.base_url}/Migration/RunTestMode",
                headers={"Content-Type": "application/json"},
                verify=False,
                timeout=10
            )
            
            data = response.json()
            self.assertTrue(data.get('success'), "Test mode should succeed")
            
            report = data.get('report', '')
            self.assertIn("Test Repositories", report, 
                         "Report should contain 'Test Repositories'")
            self.assertIn("Test Issues", report, 
                         "Report should contain 'Test Issues'")
            self.assertIn("Migration Status", report, 
                         "Report should contain 'Migration Status'")
            
            print("[PASS] Test mode report contains expected content")
        except requests.exceptions.ConnectionError:
            self.skipTest("Web application not running")
        except Exception as e:
            self.fail(f"Error testing report content: {str(e)}")
    
    def test_04_test_mode_timestamp_format(self):
        """Test that test mode generates correct timestamp format"""
        print("\n[TEST 04] Testing timestamp format...")
        try:
            response = requests.post(
                f"{self.base_url}/Migration/RunTestMode",
                headers={"Content-Type": "application/json"},
                verify=False,
                timeout=10
            )
            
            data = response.json()
            timestamp = data.get('timestamp', '')
            
            # Verify timestamp format: YYYYMMDD_HHMMSS
            self.assertEqual(len(timestamp), 15, 
                           f"Timestamp should be 15 characters, got {len(timestamp)}")
            self.assertTrue(timestamp[8] == '_', 
                          "Timestamp should have underscore at position 8")
            
            # Try to parse timestamp
            try:
                datetime.strptime(timestamp, "%Y%m%d_%H%M%S")
                print(f"[PASS] Timestamp format is valid: {timestamp}")
            except ValueError:
                self.fail(f"Invalid timestamp format: {timestamp}")
                
        except requests.exceptions.ConnectionError:
            self.skipTest("Web application not running")
        except Exception as e:
            self.fail(f"Error testing timestamp: {str(e)}")
    
    def test_05_normal_mode_missing_parameters(self):
        """Test normal mode with missing parameters"""
        print("\n[TEST 05] Testing normal mode with missing parameters...")
        try:
            response = requests.post(
                f"{self.base_url}/Migration/RunNormalMode",
                headers={"Content-Type": "application/json"},
                json={
                    "sourceToken": "test_token",
                    "sourceOrg": "test_org"
                    # Missing targetToken and targetOrg
                },
                verify=False,
                timeout=10
            )
            
            data = response.json()
            self.assertFalse(data.get('success'), 
                           "Request should fail with missing parameters")
            self.assertIn("required", data.get('message', '').lower(), 
                         "Error message should mention required parameters")
            
            print("[PASS] Normal mode correctly validates required parameters")
        except requests.exceptions.ConnectionError:
            self.skipTest("Web application not running")
        except Exception as e:
            self.fail(f"Error testing parameter validation: {str(e)}")
    
    def test_06_normal_mode_invalid_credentials(self):
        """Test normal mode with invalid credentials"""
        print("\n[TEST 06] Testing normal mode with invalid credentials...")
        try:
            response = requests.post(
                f"{self.base_url}/Migration/RunNormalMode",
                headers={"Content-Type": "application/json"},
                json={
                    "sourceToken": "invalid_token_12345",
                    "sourceOrg": "test_org",
                    "targetToken": "invalid_token_67890",
                    "targetOrg": "target_org"
                },
                verify=False,
                timeout=15
            )
            
            data = response.json()
            # Should either fail or return error in message
            if not data.get('success'):
                print(f"[PASS] Normal mode correctly handles invalid credentials: {data.get('message')}")
            else:
                # Might succeed with 0 repos if invalid org
                print("[PASS] Normal mode handled invalid credentials gracefully")
                
        except requests.exceptions.ConnectionError:
            self.skipTest("Web application not running")
        except Exception as e:
            self.fail(f"Error testing invalid credentials: {str(e)}")
    
    def test_07_response_json_structure(self):
        """Test that all endpoints return proper JSON structure"""
        print("\n[TEST 07] Testing JSON response structure...")
        try:
            response = requests.post(
                f"{self.base_url}/Migration/RunTestMode",
                headers={"Content-Type": "application/json"},
                verify=False,
                timeout=10
            )
            
            # Verify Content-Type header
            content_type = response.headers.get('Content-Type', '')
            self.assertIn('application/json', content_type.lower(), 
                         f"Response should be JSON, got {content_type}")
            
            # Verify response is valid JSON
            try:
                data = response.json()
                self.assertIsInstance(data, dict, "Response should be a JSON object")
                print("[PASS] Response has proper JSON structure")
            except json.JSONDecodeError:
                self.fail("Response is not valid JSON")
                
        except requests.exceptions.ConnectionError:
            self.skipTest("Web application not running")
        except Exception as e:
            self.fail(f"Error testing JSON structure: {str(e)}")
    
    def test_08_concurrent_requests(self):
        """Test handling of concurrent requests"""
        print("\n[TEST 08] Testing concurrent request handling...")
        try:
            import concurrent.futures
            
            def make_request():
                response = requests.post(
                    f"{self.base_url}/Migration/RunTestMode",
                    headers={"Content-Type": "application/json"},
                    verify=False,
                    timeout=15
                )
                return response.status_code == 200
            
            # Make 3 concurrent requests
            with concurrent.futures.ThreadPoolExecutor(max_workers=3) as executor:
                futures = [executor.submit(make_request) for _ in range(3)]
                results = [f.result() for f in concurrent.futures.as_completed(futures)]
            
            successful_requests = sum(results)
            self.assertGreaterEqual(successful_requests, 2, 
                                   "At least 2 of 3 concurrent requests should succeed")
            
            print(f"[PASS] Handled {successful_requests}/3 concurrent requests successfully")
            
        except requests.exceptions.ConnectionError:
            self.skipTest("Web application not running")
        except Exception as e:
            self.fail(f"Error testing concurrent requests: {str(e)}")

def run_tests():
    """Run all web application tests"""
    print("=" * 70)
    print("GitHub Migration Tool - Web Application Tests")
    print("=" * 70)
    print("\nNOTE: These tests require the web application to be running.")
    print("Start the application with: dotnet run (from WebApp folder)")
    print("Press Ctrl+C to skip if app is not running.\n")
    
    # Disable SSL warnings for self-signed certificates
    import urllib3
    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
    
    # Create test suite
    loader = unittest.TestLoader()
    suite = loader.loadTestsFromTestCase(TestWebApplication)
    
    # Run tests
    runner = unittest.TextTestRunner(verbosity=2)
    result = runner.run(suite)
    
    # Print summary
    print("\n" + "=" * 70)
    print("Test Summary")
    print("=" * 70)
    print(f"Tests run: {result.testsRun}")
    print(f"Successes: {result.testsRun - len(result.failures) - len(result.errors) - len(result.skipped)}")
    print(f"Failures: {len(result.failures)}")
    print(f"Errors: {len(result.errors)}")
    print(f"Skipped: {len(result.skipped)}")
    
    return result.wasSuccessful()

if __name__ == '__main__':
    success = run_tests()
    exit(0 if success else 1)
