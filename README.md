# git-issue-manager

A .NET application to manage internal issues on GitHub and GitLab repositories via REST API.

## Features

- Create a new issue (title and description)
- Update an existing issue
- Close an issue
- Supports GitHub and GitLab 

## Requirements

- GitHub Personal Access Token with 'repo' scope
- GitLab Personal Access Token with 'api' scope

## Configuration

1. Clone the repository.
2. Set up tokens in appsettings.Development.json:

{
  "GitHosts": {
    "GitHub": {
      "Token": "YOUR_GITHUB_TOKEN"
    },
    "GitLab": {
      "Token": "YOUR_GITLAB_TOKEN"
    }
  }
}
 
## Running the api

1. Set 'GitIssuesManager.Api' as the startup project in Visual Studio.
2. Press F5 to run in Development mode.
3. Open Swagger UI.
4. Use Swagger to test creating, updating and closing issues.

## Troubleshooting

- 403 GitHub: check your token permissions ('repo' scope)
- 404 GitLab: check the project ID or URL-encoded path
- Ensure tokens are correctly set in 'appsettings.Development.json'

