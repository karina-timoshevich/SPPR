using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WEB_2535503_Timoshevich.Domain.Models;

namespace WEB_253503_Timoshevich.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
