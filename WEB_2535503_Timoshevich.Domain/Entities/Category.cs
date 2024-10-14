using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WEB_2535503_Timoshevich.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        [JsonIgnore]
        public ICollection<Dish> Dishes { get; set; }

        public Category()
        {
            Dishes = new List<Dish>();
        }
    }
}
