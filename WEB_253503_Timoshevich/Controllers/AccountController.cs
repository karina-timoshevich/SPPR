using Microsoft.AspNetCore.Mvc;

namespace WEB_253503_Timoshevich.UI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
