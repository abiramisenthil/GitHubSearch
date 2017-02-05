using GitHub.Services.Model;
using GitHub.Services.Repository;
using System.Linq;
using System;
namespace GitHub.Services.Services
{
    public class GitHubServices
    {
        public GitHubServices()
        {
            repository = new GitHubRepository();
        }

        public GitHubServices(IGitHubRepository gitHubRepository)
        {
            repository = gitHubRepository;
        }

        private IGitHubRepository repository { get; set; }

        public GitHubUserInformationResponse Process(string userName)
        {
            var response = new GitHubUserInformationResponse();
            response.UserName = Convert.ToString(userName);
            var userDetails = repository.GetUserDetailsOnGitHub(userName);
            if (userDetails != null)
            {
                if (userDetails.Status == Model.ResponseStatus.Success)
                {
                    if (userDetails.avatar_url != null)
                    {
                        response.AvatarURL = userDetails.avatar_url;
                    }
                    if (userDetails.location != null)
                    {
                        response.Location = userDetails.location;
                    }

                    response.UserName = userDetails.login;
                    var result = repository.GetUserRepoDetailsOnGitHub(userDetails.repos_url);
                    if (result != null)
                    {
                        if (result.Status == Model.ResponseStatus.Success)
                        {
                            if (result.RepositoryDetailsList != null && result.RepositoryDetailsList.Any())
                                response.RepositoryDetails.AddRange(result.RepositoryDetailsList.OrderByDescending(O => O.stargazers_count).Take(5));
                            response.Status = ResponseStatus.Success;
                        }
                        else
                        {
                            response.Status = result.Status;
                            response.ErrorMessage = result.ErrorMessage;
                        }
                    }
                    else
                    {
                        response.Status = ResponseStatus.Fail;
                        response.ErrorMessage = "GitHubService: Repo Details Result is Null";
                    }
                }
                else
                {
                    response.Status = userDetails.Status;
                    response.ErrorMessage = userDetails.ErrorMessage;
                }
            }
            else
            {
                response.Status = ResponseStatus.Fail;
                response.ErrorMessage = "GitHubService: User Details Result is Null";
            }
            return response;
        }
    }
}