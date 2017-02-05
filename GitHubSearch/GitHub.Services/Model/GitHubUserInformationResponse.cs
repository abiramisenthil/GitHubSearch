using System.Collections.Generic;

namespace GitHub.Services.Model
{
    public class GitHubUserInformationResponse : SharedRequest
    {
        public GitHubUserInformationResponse()
        {
            RepositoryDetails = new List<RepositoryDetail>();
        }

        public string UserName { get; set; }
        public string Location { get; set; }
        public string AvatarURL { get; set; }
        public List<RepositoryDetail> RepositoryDetails { get; set; }
    }
}