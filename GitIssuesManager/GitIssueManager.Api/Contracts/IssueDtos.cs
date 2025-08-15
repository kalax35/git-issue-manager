namespace GitIssueManager.Api.Contracts;

public record CreateIssueRequest(string Repo, string Title, string Description);
public record UpdateIssueRequest(string Repo, int Number, string Title, string Description);
