using GitIssueManager.Api.Contracts;
using GitIssueManager.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GitIssueManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueServiceResolver _resolver;

        public IssuesController(IIssueServiceResolver resolver) => _resolver = resolver;

        [HttpPost("{platform}")] // POST api/issues/github
        public async Task<IActionResult> Create(string platform, [FromBody] CreateIssueRequest req, CancellationToken ct)
        {
            var svc = _resolver.Resolve(platform);
            var issue = await svc.CreateIssueAsync(req.Repo, req.Title, req.Description, ct);
            return Ok(issue);
        }

        [HttpPut("{platform}")] // PUT api/issues/gitlab
        public async Task<IActionResult> Update(string platform, [FromBody] UpdateIssueRequest req, CancellationToken ct)
        {
            var svc = _resolver.Resolve(platform);
            var issue = await svc.UpdateIssueAsync(req.Repo, req.Number, req.Title, req.Description, ct);
            return Ok(issue);
        }

        [HttpPost("{platform}/{number}/close")] // POST api/issues/github/123/close
        public async Task<IActionResult> Close(string platform, int number, [FromQuery] string repo, CancellationToken ct)
        {
            var svc = _resolver.Resolve(platform);
            await svc.CloseIssueAsync(repo, number, ct);
            return NoContent();
        }
    }
}
