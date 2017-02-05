using GitHub.Services.Model;

namespace GitHub.Services.Repository
{
    public interface IGitHubRepository
    {
        GitHubUserDetailsResponse GetUserDetailsOnGitHub(string userName);

        GitHubUserRepoDetailsResponse GetUserRepoDetailsOnGitHub(string url);
    }
}