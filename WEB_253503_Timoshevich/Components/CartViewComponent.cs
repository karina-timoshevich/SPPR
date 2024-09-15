using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WEB_253503_Timoshevich.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cartInfo = new
            {
                TotalPrice = 0.0,
                ItemsCount = 0
            };

            return View(cartInfo);
        }
    }
}
