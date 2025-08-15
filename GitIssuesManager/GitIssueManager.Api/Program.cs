using GitIssueManager.Core.Abstractions;
using GitIssueManager.Core.Infrastructure;
using GitIssueManager.Core.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<IIssueServiceResolver, IssueServiceResolver>();

builder.Services.AddHttpClient<GitHubIssueService>(client =>
{
    client.BaseAddress = new Uri("https://api.github.com/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("GitIssuesManager/1.0");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
    client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
    var token = cfg["GitHosts:GitHub:Token"];
    if (!string.IsNullOrWhiteSpace(token))
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
});

builder.Services.AddHttpClient<GitLabIssueService>(client =>
{
    client.BaseAddress = new Uri("https://gitlab.com/api/v4/");
    var token = cfg["GitHosts:GitLab:Token"];
    if (!string.IsNullOrWhiteSpace(token))
        client.DefaultRequestHeaders.Add("PRIVATE-TOKEN", token);
});

builder.Services.AddTransient<IIssueService>(sp => sp.GetRequiredService<GitHubIssueService>());
builder.Services.AddTransient<IIssueService>(sp => sp.GetRequiredService<GitLabIssueService>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
