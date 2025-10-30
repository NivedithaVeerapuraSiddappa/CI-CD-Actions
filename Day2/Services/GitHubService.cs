using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using GitHubMigrationTool.Models;

namespace GitHubMigrationTool.Services
{
    public class GitHubService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _personalAccessToken;

        public GitHubService(string personalAccessToken)
        {
            _personalAccessToken = personalAccessToken;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.github.com/");
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitHubMigrationTool", "1.0"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _personalAccessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        }

        public async Task<List<Repository>> GetRepositoriesAsync(string owner)
        {
            try
            {
                var response = await _httpClient.GetAsync($"users/{owner}/repos?per_page=100&type=owner");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var repos = JsonSerializer.Deserialize<List<Repository>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return repos ?? new List<Repository>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching repositories for {owner}: {ex.Message}");
                return new List<Repository>();
            }
        }

        public async Task<bool> CreateRepositoryAsync(string repoName, string description, bool isPrivate)
        {
            try
            {
                var payload = new
                {
                    name = repoName,
                    description = description ?? "Migrated repository",
                    @private = isPrivate,
                    auto_init = false
                };

                var jsonContent = JsonSerializer.Serialize(payload);
                var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("user/repos", httpContent);
                
                if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    Console.WriteLine($"Repository '{repoName}' already exists or name is invalid");
                    return false;
                }
                
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Created repository: {repoName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating repository {repoName}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Issue>> GetIssuesAsync(string owner, string repo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"repos/{owner}/{repo}/issues?state=all&per_page=100");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var issues = JsonSerializer.Deserialize<List<Issue>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return issues ?? new List<Issue>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching issues for {repo}: {ex.Message}");
                return new List<Issue>();
            }
        }

        public async Task<bool> CreateIssueAsync(string owner, string repo, string title, string body, List<string> labels)
        {
            try
            {
                var payload = new
                {
                    title = title,
                    body = body ?? "",
                    labels = labels ?? new List<string>()
                };

                var jsonContent = JsonSerializer.Serialize(payload);
                var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"repos/{owner}/{repo}/issues", httpContent);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating issue '{title}': {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
