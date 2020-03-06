using System.Web.Mvc;

namespace TMS.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/Help");
        }
    }
}