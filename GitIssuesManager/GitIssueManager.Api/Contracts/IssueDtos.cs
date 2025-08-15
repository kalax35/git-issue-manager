namespace GitIssueManager.Api.Contracts;

/// <summary>
/// Request to create a new issue
/// </summary>
public class CreateIssueRequest
{
    /// <summary>
    /// The full repository name, e.g. "user/repo"
    /// </summary>
    public string Repo { get; set; }

    /// <summary>
    /// Title of the issue
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Detailed description of the issue
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// Request to update an existing issue
/// </summary>
public class UpdateIssueRequest
{
    /// <summary>
    /// The full repository name, e.g. "user/repo"
    /// </summary>
    public string Repo { get; set; }

    /// <summary>
    /// Updated title of the issue
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Updated description of the issue
    /// </summary>
    public string Description { get; set; }
}

