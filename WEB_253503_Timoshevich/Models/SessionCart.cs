using System.Text.Json.Serialization;
using WEB_253503_Timoshevich.UI.Extensions;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;

namespace WEB_253503_Timoshevich.UI.Models
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        private IHttpContextAccessor _httpContextAccessor;

        // public new int Count { get => _cart.Count; }
        //
        // public new double TotalPrice
        // {
        //     get => _cart.TotalPrice;
        // }

        public SessionCart() { }

        public SessionCart(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static Cart GetCart(IServiceProvider services)
        {
            var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            var session = httpContext.Session;

            var cart = session.Get<SessionCart>("cart") ?? new SessionCart(httpContextAccessor);
            cart._httpContextAccessor = httpContextAccessor;
            return cart;
        }

        public override void AddToCart(Dish dish)
        {
            base.AddToCart(dish);
            _httpContextAccessor.HttpContext.Session.Set("cart", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            _httpContextAccessor.HttpContext.Session.Set("cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            _httpContextAccessor.HttpContext.Session.Set("cart", this);
        }
    }
}
