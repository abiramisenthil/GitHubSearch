using GitHub.Services.Model;
using GitHub.Services.Repository;
using GitHub.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GitHub.Services.Tests.ServicesTest
{
    [TestClass]
    public class GitHubServicesUnitTest
    {
        public GitHubServicesUnitTest()
        {
            BuildMockGitHubRepository();
        }

        private Mock<IGitHubRepository> moqRepository = new Mock<IGitHubRepository>();

        [TestMethod]
        public void GitHubServices_UserNameAsNull_TestCase1()
        {
            GitHubServices services = new GitHubServices(moqRepository.Object);

            var response = services.Process(string.Empty);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Fail);
            Assert.AreEqual(response.ErrorMessage, "GitHubService: User Details Result is Null");
        }

        [TestMethod]
        public void GitHubServices_UserNameAsEmpty_TestCase2()
        {
            GitHubServices services = new GitHubServices(moqRepository.Object);

            var response = services.Process(string.Empty);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Fail);
            Assert.AreEqual(response.ErrorMessage, "GitHubService: User Details Result is Null");
        }

        [TestMethod]
        public void GitHubServices_UserNameAsInValid_TestCase3()
        {
            GitHubServices services = new GitHubServices(moqRepository.Object);

            var response = services.Process("-1");

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Fail);
            Assert.AreEqual(response.ErrorMessage, "User Not Found");
        }

        [TestMethod]
        public void GitHubServices_UserNameAsValidWithRepoUrlAsNull_TestCase4()
        {
            GitHubServices services = new GitHubServices(moqRepository.Object);

            var response = services.Process("RobconeryWithRopeUrlNull");

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Fail);
            Assert.AreEqual(response.ErrorMessage, "GitHubService: Repo Details Result is Null");
        }

        [TestMethod]
        public void GitHubServices_UserNameAsValidWithRepoUrlInValid_TestCase5()
        {
            GitHubServices services = new GitHubServices(moqRepository.Object);

            var response = services.Process("RobconeryInValidURL");

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Error);
            Assert.AreEqual(response.ErrorMessage, "GetUserRepoDetailsOnGitHub: Exception Occurred During API Call");
            Assert.IsNotNull(response.RepositoryDetails);
            Assert.IsFalse(response.RepositoryDetails.Any());
        }

        [TestMethod]
        public void GitHubServices_UserNameAsValidWithRepoUrlValid_TestCase6()
        {
            GitHubServices services = new GitHubServices(moqRepository.Object);

            var response = services.Process("Robconery");

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Success);
            Assert.IsTrue(string.IsNullOrEmpty(response.ErrorMessage));
            Assert.IsNotNull(response.RepositoryDetails);
            Assert.IsTrue(response.RepositoryDetails.Any());
        }

        private void BuildMockGitHubRepository()
        {
            GitHubUserDetailsResponse moqResponse = new GitHubUserDetailsResponse();
            GitHubUserRepoDetailsResponse moqRepoDetailsResponse = new GitHubUserRepoDetailsResponse();
            moqResponse = null;
            moqRepository.Setup(f => f.GetUserDetailsOnGitHub(null)).Returns(moqResponse);
            moqRepository.Setup(f => f.GetUserDetailsOnGitHub(string.Empty)).Returns(moqResponse);

            moqResponse = new GitHubUserDetailsResponse();
            moqResponse.ErrorMessage = "User Not Found";
            moqResponse.Status = ResponseStatus.Fail;
            moqRepository.Setup(f => f.GetUserDetailsOnGitHub("-1")).Returns(moqResponse);

            moqResponse = new GitHubUserDetailsResponse();
            moqResponse.Status = ResponseStatus.Success;

            moqResponse.avatar_url = "https://avatars.githubusercontent.com/u/78586?v=3";
            moqResponse.location = "Seattle, WA";
            moqResponse.login = "robconery";
            moqResponse.repos_url = null;

            moqRepository.Setup(f => f.GetUserDetailsOnGitHub("RobconeryWithRopeUrlNull")).Returns(moqResponse);

            moqResponse = new GitHubUserDetailsResponse();
            moqResponse.Status = ResponseStatus.Success;

            moqResponse.avatar_url = "https://avatars.githubusercontent.com/u/78586?v=3";
            moqResponse.location = "Seattle, WA";
            moqResponse.login = "robconery";
            moqResponse.repos_url = "https://api.github.com/users/RobconeryInValidURL/repos";

            moqRepository.Setup(f => f.GetUserDetailsOnGitHub("RobconeryInValidURL")).Returns(moqResponse);

            moqRepoDetailsResponse = new GitHubUserRepoDetailsResponse();
            moqRepoDetailsResponse.Status = ResponseStatus.Error;
            moqRepoDetailsResponse.RepositoryDetailsList = null;
            moqRepoDetailsResponse.ErrorMessage = "GetUserRepoDetailsOnGitHub: Exception Occurred During API Call";
            moqRepository.Setup(f => f.GetUserRepoDetailsOnGitHub(moqResponse.repos_url)).Returns(moqRepoDetailsResponse);

            moqResponse = new GitHubUserDetailsResponse();
            moqResponse.Status = ResponseStatus.Success;

            moqResponse.avatar_url = "https://avatars.githubusercontent.com/u/78586?v=3";
            moqResponse.location = "Seattle, WA";
            moqResponse.login = "robconery";
            moqResponse.repos_url = "https://api.github.com/users/robconery/repos";

            moqRepository.Setup(f => f.GetUserDetailsOnGitHub("Robconery")).Returns(moqResponse);

            moqRepoDetailsResponse = new GitHubUserRepoDetailsResponse();
            moqRepoDetailsResponse.Status = ResponseStatus.Success;
            moqRepoDetailsResponse.RepositoryDetailsList = GetStubbRepoDetails();
            moqRepository.Setup(f => f.GetUserRepoDetailsOnGitHub(moqResponse.repos_url)).Returns(moqRepoDetailsResponse);
        }

        private RepositoryDetail[] GetStubbRepoDetails()
        {
            var randomNo = new Random();
            randomNo.Next();
            List<RepositoryDetail> responseList = new List<RepositoryDetail>();
            for (int i = 0; i < 5; i++)
            {
                responseList.Add(new RepositoryDetail() { stargazers_count = randomNo.Next() });
            }

            return responseList.ToArray();
        }
    }
}