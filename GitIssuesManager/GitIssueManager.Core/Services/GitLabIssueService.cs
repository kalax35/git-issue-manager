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
    public class GitLabIssueService : IIssueService
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, //gitlab return snake_case
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public string Platform => "gitlab";

        public GitLabIssueService(HttpClient http) => _http = http;

        public async Task<Issue> CreateIssueAsync(string repo, string title, string description, CancellationToken ct = default)
        {
            var projectId = EncodeProjectId(repo);
            var url = $"projects/{projectId}/issues";
            var payload = new { title, description };

            using var resp = await _http.PostAsJsonAsync(url, payload, JsonOpts, ct);
            await EnsureSuccess(resp);

            var gl = await resp.Content.ReadFromJsonAsync<GitLabIssueDto>(JsonOpts, ct);
            return Map(gl!);
        }

        public async Task<Issue> UpdateIssueAsync(string repo, int issueNumber, string title, string description, CancellationToken ct = default)
        {
            var projectId = EncodeProjectId(repo);
            var url = $"projects/{projectId}/issues/{issueNumber}";
            var payload = new { title, description };

            using var resp = await _http.PutAsJsonAsync(url, payload, JsonOpts, ct);
            await EnsureSuccess(resp);

            var gl = await resp.Content.ReadFromJsonAsync<GitLabIssueDto>(JsonOpts, ct);
            return Map(gl!);
        }

        public async Task CloseIssueAsync(string repo, int issueNumber, CancellationToken ct = default)
        {
            var projectId = EncodeProjectId(repo);
            var url = $"projects/{projectId}/issues/{issueNumber}";
            var payload = new { state_event = "close" };

            using var resp = await _http.PutAsJsonAsync(url, payload, JsonOpts, ct);
            await EnsureSuccess(resp);
        }

        private static string EncodeProjectId(string repo)
        {
            //gitLab akceptuje ID liczbowe lub URL-encoded ścieżkę namespace/projekt
            if (int.TryParse(repo, out _)) return repo;
            return Uri.EscapeDataString(repo);
        }

        private static async Task EnsureSuccess(HttpResponseMessage resp)
        {
            if (!resp.IsSuccessStatusCode)
            {
                var content = await resp.Content.ReadAsStringAsync();
                throw new HttpRequestException($"GitLab API error {(int)resp.StatusCode}: {content}");
            }
        }

        private static Issue Map(GitLabIssueDto gl) => new()
        {
            Number = gl.iid,
            Title = gl.title ?? "",
            Description = gl.description ?? "",
            State = gl.state ?? "",
            WebUrl = gl.web_url ?? ""
        };

        private record GitLabIssueDto(
            int iid,
            string? title,
            string? description,
            string? state,
            string? web_url
        );
    }
}
