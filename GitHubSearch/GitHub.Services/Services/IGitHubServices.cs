using GitHub.Services.Model;

namespace GitHub.Services.Services
{
    public interface IGitHubServices
    {
        GitHubUserInformationResponse Process(string userName);
    }
}