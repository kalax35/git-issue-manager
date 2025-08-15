using GitIssueManager.Core.Abstractions;
using GitIssueManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitIssueManager.Core.Services
{
    public class GitHubIssueService : IIssueService
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public string Platform => "github";

        public GitHubIssueService(HttpClient http) => _http = http;

        public async Task<Issue> CreateIssueAsync(string repo, string title, string description, CancellationToken ct = default)
        {
            var (owner, name) = SplitOwnerRepo(repo);
            var url = $"repos/{owner}/{name}/issues";
            var payload = new { title, body = description };

            using var resp = await _http.PostAsJsonAsync(url, payload, JsonOpts, ct);
            await EnsureSuccess(resp);

            var gh = await resp.Content.ReadFromJsonAsync<GitHubIssueDto>(JsonOpts, ct);
            return Map(gh!);
        }

        public async Task<Issue> UpdateIssueAsync(string repo, int issueNumber, string title, string description, CancellationToken ct = default)
        {
            var (owner, name) = SplitOwnerRepo(repo);
            var url = $"repos/{owner}/{name}/issues/{issueNumber}";
            var payload = new { title, body = description };

            using var req = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = JsonContent.Create(payload, options: JsonOpts)
            };
            using var resp = await _http.SendAsync(req, ct);
            await EnsureSuccess(resp);

            var gh = await resp.Content.ReadFromJsonAsync<GitHubIssueDto>(JsonOpts, ct);
            return Map(gh!);
        }

        public async Task CloseIssueAsync(string repo, int issueNumber, CancellationToken ct = default)
        {
            var (owner, name) = SplitOwnerRepo(repo);
            var url = $"repos/{owner}/{name}/issues/{issueNumber}";
            var payload = new { state = "closed" };

            using var req = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = JsonContent.Create(payload, options: JsonOpts)
            };
            using var resp = await _http.SendAsync(req, ct);
            await EnsureSuccess(resp);
        }

        private static (string owner, string name) SplitOwnerRepo(string repo)
        {
            var parts = repo.Split('/', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) throw new ArgumentException("Repo must be 'owner/name'.");
            return (parts[0], parts[1]);
        }

        private static async Task EnsureSuccess(HttpResponseMessage resp)
        {
            if (!resp.IsSuccessStatusCode)
            {
                var content = await resp.Content.ReadAsStringAsync();
                throw new HttpRequestException($"GitHub API error {(int)resp.StatusCode}: {content}");
            }
        }

        private static Issue Map(GitHubIssueDto gh) => new()
        {
            Number = gh.number,
            Title = gh.title ?? "",
            Description = gh.body ?? "",
            State = gh.state ?? "",
            WebUrl = gh.html_url ?? ""
        };

        private record GitHubIssueDto(
            int number,
            string? title,
            string? body,
            string? state,
            string? html_url
        );
    }
}
