using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_2535503_Timoshevich.Domain.Models;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_253503_Timoshevich.UI.Services.CategoryService;

namespace WEB_253503_Timoshevich.UI.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        private List<Dish> _dishes;
        private List<Category> _categories;
        private readonly int _itemsPerPage;

        public MemoryProductService(ICategoryService categoryService, IConfiguration config)
        {
            // Получаем категории через categoryService
            _categories = categoryService.GetCategoryListAsync().Result.Data;

            // Получаем количество элементов на странице из конфигурации
            _itemsPerPage = int.Parse(config["ItemsPerPage"] ?? "3");

            // Заполняем коллекции данными
            SetupData();
        }




        /// <summary>
        /// Инициализация списков с тестовыми данными
        /// </summary>
        private void SetupData()
        {
            _dishes = new List<Dish>
        {
            new Dish { Id = 1, Name = "Soup harcho",
                Description = "Very острый, not cool",
                Calories = 200, Image = "Images/Soup.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
            },
            new Dish { Id = 2, Name = "Borsch",
                Description = "A lot of salo, without сметаны",
                Calories = 330, Image = "Images/Borsch.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
            },
            new Dish { Id = 3, Name = "Krabovyi",
                Description = "Salad with krabovye palochki",
                Calories = 400, Image = "Images/Krabovyi.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("salads"))
            },
            new Dish { Id = 4, Name = "Tea",
                Description = "Hot drink",
                Calories = 120, Image = "Images/Tea.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("drinks"))
            },
            new Dish { Id = 5, Name = "Cake",
                Description = "Coll dessert",
                Calories = 500, Image = "Images/Cake.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("desserts"))
            }
        };
        }

        /// <summary>
        /// Получение списка продуктов с фильтрацией по категории и разбиением на страницы
        /// </summary>
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var filteredDishes = _dishes.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName)).ToList();
            var result = new ListModel<Dish>
            {
                Items = filteredDishes,
                CurrentPage = pageNo,
                TotalPages = 1 // В этой реализации пагинация не учитывается, но можно добавить
            };
            return Task.FromResult(ResponseData<ListModel<Dish>>.Success(result));
        }
    }
}
