using GitIssueManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitIssueManager.Core.Interfaces
{
    public interface IIssueService
    {
        Task<Issue> CreateIssueAsync(string repo, string title, string description);
        Task<Issue> UpdateIssueAsync(string repo, int issueId, string title, string description);
        Task CloseIssueAsync(string repo, int issueId);
    }
}
