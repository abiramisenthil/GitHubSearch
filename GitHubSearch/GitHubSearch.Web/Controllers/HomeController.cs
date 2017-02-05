using GitHub.Services.Services;
using System.Web.Mvc;

namespace GitHubSearch.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Home");
        }

        [HttpPost]
        public ActionResult GetUserDetails(string txtUsername)
        {
            if (!string.IsNullOrEmpty(txtUsername))
            {
                GitHubServices services = new GitHubServices();
                var response = services.Process(txtUsername);
                return View("UserDetails", response);
            }
            else
            {
                return View("Home");
            }
        }
    }
}