using GitHub.Services.Model;
using GitHub.Services.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitHub.Services.Tests.RepositoryTest
{
    [TestClass]
    public class GitHubRepositoryUnitTest
    {
        #region GetUserDetailsOnGitHub Test Methods

        [TestMethod]
        public void GetUserDetailsOnGitHub_UserNameEmpty_TestCase1()
        {
            GitHubRepository repository = new GitHubRepository();
            var response = repository.GetUserDetailsOnGitHub(string.Empty);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.InValidInput);
            Assert.AreEqual(response.ErrorMessage, "UserName Cant be Empty");
        }

        [TestMethod]
        public void GetUserDetailsOnGitHub_UserNameNull_TestCase2()
        {
            GitHubRepository repository = new GitHubRepository();
            var response = repository.GetUserDetailsOnGitHub(null);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.InValidInput);
            Assert.AreEqual(response.ErrorMessage, "UserName Cant be Empty");
        }

        [TestMethod]
        public void GetUserDetailsOnGitHub_UserNameNotFound_TestCase3()
        {
            GitHubRepository repository = new GitHubRepository();
            var response = repository.GetUserDetailsOnGitHub("-1");

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Fail);
            Assert.AreEqual(response.ErrorMessage, "User Not Found");
        }

        [TestMethod]
        public void GetUserDetailsOnGitHub_ValidUserName_TestCase4()
        {
            GitHubRepository repository = new GitHubRepository();
            var response = repository.GetUserDetailsOnGitHub("Robconery");

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Success);
            Assert.IsTrue(string.IsNullOrEmpty(response.ErrorMessage));
        }

        #endregion GetUserDetailsOnGitHub Test Methods

        #region GetUserRepoDetailsOnGitHub Test Methods

        [TestMethod]
        public void GetUserRepoDetailsOnGitHub_URLEmpty_TestCase1()
        {
            GitHubRepository repository = new GitHubRepository();
            var response = repository.GetUserRepoDetailsOnGitHub(string.Empty);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.InValidInput);
            Assert.AreEqual(response.ErrorMessage, "URL Cant be Empty");
        }

        [TestMethod]
        public void GetUserRepoDetailsOnGitHub_URLEmpty_TestCase2()
        {
            GitHubRepository repository = new GitHubRepository();
            var response = repository.GetUserRepoDetailsOnGitHub(null);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.InValidInput);
            Assert.AreEqual(response.ErrorMessage, "URL Cant be Empty");
        }

        [TestMethod]
        public void GetUserRepoDetailsOnGitHub_URLNotFound_TestCase3()
        {
            GitHubRepository repository = new GitHubRepository();
            var response = repository.GetUserRepoDetailsOnGitHub("https://api.github.com/users/robconeryasdasdasdasd/repos");

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Error);
            Assert.AreEqual(response.ErrorMessage, "GetUserRepoDetailsOnGitHub: Exception Occurred During API Call");
        }

        [TestMethod]
        public void GetUserRepoDetailsOnGitHub_ValidURL_TestCase4()
        {
            GitHubRepository repository = new GitHubRepository();
            var response = repository.GetUserRepoDetailsOnGitHub(@"https://api.github.com/users/robconery/repos");

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, ResponseStatus.Success);
            Assert.IsTrue(string.IsNullOrEmpty(response.ErrorMessage));
        }

        #endregion GetUserRepoDetailsOnGitHub Test Methods
    }
}