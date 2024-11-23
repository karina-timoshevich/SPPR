using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_2535503_Timoshevich.Domain.Entities;

namespace WEB_2535503_Timoshevich.Domain.Models
{
    public class CartItem
    {
        public Dish Dish { get; set; } 
        public int Count { get; set; } = 1; 
    }
}
