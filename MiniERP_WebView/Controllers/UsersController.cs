using Microsoft.AspNetCore.Mvc;

namespace MiniERP_WebView.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
