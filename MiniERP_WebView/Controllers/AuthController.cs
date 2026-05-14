using Microsoft.AspNetCore.Mvc;

namespace MiniERP_WebView.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config) => _config = config;

        public IActionResult Login()
        {
            ViewBag.ApiBase = _config["ApiSettings:BaseUrl"];
            return View();
        }

        public IActionResult Register()
        {
            ViewBag.ApiBase = _config["ApiSettings:BaseUrl"];
            return View();
        }
    }
}
