using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeavyGo_Project.Controllers
{
    public class LandingPageController : Controller
    {
        // GET: LandingPageController
        public ActionResult Index()
        {
            return View();
        }

    }
}
