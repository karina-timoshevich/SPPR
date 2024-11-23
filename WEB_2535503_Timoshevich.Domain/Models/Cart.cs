using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_2535503_Timoshevich.Domain.Entities;

namespace WEB_2535503_Timoshevich.Domain.Models
{

    public class Cart
    {
        public List<CartItem> CartItems { get; set; }
        public int TotalPrice
        {
            get => CartItems.Sum(item => (int)(item.Dish.Price * item.Count));
        }


        public int Count
        {
            get => CartItems.Sum(item => item.Count);
        }

        public Cart()
        {
            CartItems = new List<CartItem>();
        }

        public virtual void AddToCart(Dish dish)
        {
            var cartItem = CartItems.FirstOrDefault(item => item.Dish.Id == dish.Id);
            if (cartItem is null)
            {
                CartItems.Add(new CartItem { Dish = dish, Count = 1 });
            }
            else
            {
                cartItem.Count++;
            }
        }

        public virtual void RemoveItems(int id)
        {
            var cartItem = CartItems.FirstOrDefault(item => item.Dish.Id == id);
            if (cartItem is not null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                }
                else
                {
                    CartItems.Remove(cartItem);
                }
            }
        }

        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
    }
}
