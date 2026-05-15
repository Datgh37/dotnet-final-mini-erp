using Microsoft.AspNetCore.Mvc;

namespace MiniERP_WebView.Controllers
{
    public class SalesOrdersController : Controller
    {
        // GET: /SalesOrders/
        public IActionResult Index()
        {
            return View();
        }
    }
}
