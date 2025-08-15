using GitIssueManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitIssueManager.Core.Abstractions
{
    public interface IIssueService
    {
        Task<Issue> CreateIssueAsync(string repo, string title, string description, CancellationToken ct = default);
        Task<Issue> UpdateIssueAsync(string repo, int issueNumber, string title, string description, CancellationToken ct = default);
        Task CloseIssueAsync(string repo, int issueNumber, CancellationToken ct = default);
        string Platform { get; }
    }
}
