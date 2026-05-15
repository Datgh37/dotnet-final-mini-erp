using Microsoft.AspNetCore.Mvc;

namespace MiniERP_WebView.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
