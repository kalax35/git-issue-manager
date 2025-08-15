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

        /// <summary>
        /// Creates a new issue in the given repository
        /// </summary>
        [HttpPost("{platform}")] //POST
        public async Task<IActionResult> Create(string platform, [FromBody] CreateIssueRequest req, CancellationToken ct)
        {
            var svc = _resolver.Resolve(platform);
            var issue = await svc.CreateIssueAsync(req.Repo, req.Title, req.Description, ct);
            return Ok(issue);
        }

        /// <summary>
        /// Updates an existing issue's title and description
        /// </summary>
        [HttpPut("{platform}/{number}")] //PUT
        public async Task<IActionResult> Update(string platform, int number, [FromBody] UpdateIssueRequest req, CancellationToken ct)
        {
            var svc = _resolver.Resolve(platform);
            var issue = await svc.UpdateIssueAsync(req.Repo, number, req.Title, req.Description, ct);
            return Ok(issue);
        }

        /// <summary>
        /// Updates an existing issue's title and description
        /// </summary>
        [HttpPost("{platform}/{number}/close")] //POST
        public async Task<IActionResult> Close(string platform, int number, [FromQuery] string repo, CancellationToken ct)
        {
            var svc = _resolver.Resolve(platform);
            await svc.CloseIssueAsync(repo, number, ct);
            return NoContent();
        }
    }
}
