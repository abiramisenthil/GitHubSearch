using System.Runtime.Serialization;

namespace GitHub.Services.Model
{
    [DataContract]
    public class GitHubUserDetailsResponse : SharedRequest
    {
        [DataMember(Name = "login")]
        public string login { get; set; }

        [DataMember(Name = "avatar_url")]
        public string avatar_url { get; set; }

        [DataMember(Name = "location")]
        public string location { get; set; }

        [DataMember(Name = "repos_url")]
        public string repos_url { get; set; }

        /// <summary>
        /// If User Not Found Then We return Message here
        /// </summary>
        [DataMember(Name = "message")]
        public string message { get; set; }
    }
}