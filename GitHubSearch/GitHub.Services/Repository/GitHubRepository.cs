using GitHub.Services.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GitHub.Services.Repository
{
    public class GitHubRepository : IGitHubRepository
    {
        public GitHubRepository()
        {
        }

        private string GetGitHubUserDetailsBaseURL
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings["GitHubUserDetailsURL"]);
            }
        }

        public GitHubUserDetailsResponse GetUserDetailsOnGitHub(string userName)
        {
            var response = new GitHubUserDetailsResponse();
            if (string.IsNullOrEmpty(userName))
            {
                response.Status = ResponseStatus.InValidInput;
                response.ErrorMessage = "UserName Cant be Empty";
                return response;
            }
            try
            {
                response = WebClientCall<GitHubUserDetailsResponse>(string.Format(GetGitHubUserDetailsBaseURL, userName));
                if (string.IsNullOrEmpty(response.message))
                {
                    response.Status = ResponseStatus.Success;
                    response.ErrorMessage = string.Empty;
                }
                else
                {
                    response.Status = ResponseStatus.Fail;
                    response.ErrorMessage = "User Not Found";
                }
            }
            catch
            {
                response.Status = ResponseStatus.Error;
                response.ErrorMessage = "GetUserDetailsOnGitHub: Exception Occurred During API Call";
            }
            return response;
        }

        public GitHubUserRepoDetailsResponse GetUserRepoDetailsOnGitHub(string url)
        {
            var response = new GitHubUserRepoDetailsResponse();
            if (string.IsNullOrEmpty(url))
            {
                response.Status = ResponseStatus.InValidInput;
                response.ErrorMessage = "URL Cant be Empty";
                return response;
            }
            try
            {
                response.RepositoryDetailsList = WebClientCall<RepositoryDetail[]>(url);

                response.Status = ResponseStatus.Success;
                response.ErrorMessage = string.Empty;
            }
            catch
            {
                response.Status = ResponseStatus.Error;
                response.ErrorMessage = "GetUserRepoDetailsOnGitHub: Exception Occurred During API Call";
            }
            return response;
        }

        private T WebClientCall<T>(string url)
        {
            try
            {
                var apiResponse = default(T);

                var cl = new HttpClient();
                cl.BaseAddress = new Uri(url);
                int _TimeoutSec = 90;
                cl.Timeout = new TimeSpan(0, 0, _TimeoutSec);
                cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cl.DefaultRequestHeaders.Add("User-Agent", "GitHubCall");

                var task = cl.GetAsync(url)
                  .ContinueWith((taskwithresponse) =>
                  {
                      var response = taskwithresponse.Result;
                      var jsonString = response.Content.ReadAsStringAsync();
                      jsonString.Wait();
                      apiResponse = JsonConvert.DeserializeObject<T>(jsonString.Result);
                  });
                task.Wait();

                return apiResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}