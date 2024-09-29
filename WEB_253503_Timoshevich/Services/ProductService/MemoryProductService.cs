using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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

        // Внедряем IConfiguration и ICategoryService в конструктор
        public MemoryProductService(
            IConfiguration config,
            ICategoryService categoryService)
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
                },
                new Dish { Id = 6, Name = "Chicken soup",
                    Description = "Soup kak y granny",
                    Calories = 500, Image = "Images/ChickenSoup.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
                },
                new Dish { Id = 7, Name = "Ramen", // Исправлено ID
                    Description = "Imagine Asia",
                    Calories = 500, Image = "Images/Ramen.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
                }
            };
        }

        /// <summary>
        /// Получение списка продуктов с фильтрацией по категории и разбиением на страницы
        /// </summary>
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // Проверяем, если categoryNormalizedName == null или пустая строка, то возвращаем все блюда
            var filteredDishes = _dishes
                .Where(d => string.IsNullOrEmpty(categoryNormalizedName) || categoryNormalizedName == "Все" || d.Category.NormalizedName.Equals(categoryNormalizedName))
                .ToList();

            // Вычисляем общее количество страниц
            var totalPages = (int)Math.Ceiling((double)filteredDishes.Count / _itemsPerPage);

            // Убедитесь, что pageNo находится в допустимых пределах
            if (pageNo < 1 || pageNo > totalPages)
            {
                pageNo = 1; // Сбрасываем на первую страницу, если номер вне пределов
            }

            // Получаем нужную страницу
            var paginatedDishes = filteredDishes
                .Skip((pageNo - 1) * _itemsPerPage) // Пропускаем элементы до нужной страницы
                .Take(_itemsPerPage) // Берем только количество элементов на страницу
                .ToList();

            var result = new ListModel<Dish>
            {
                Items = paginatedDishes,
                CurrentPage = pageNo,
                TotalPages = totalPages // Устанавливаем общее количество страниц
            };

            return Task.FromResult(ResponseData<ListModel<Dish>>.Success(result));
        }


    }
}
