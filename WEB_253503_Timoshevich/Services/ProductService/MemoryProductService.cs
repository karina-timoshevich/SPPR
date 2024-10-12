﻿using System.Collections.Generic;
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

        public MemoryProductService(
            IConfiguration config,
            ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            _itemsPerPage = int.Parse(config["ItemsPerPage"] ?? "3");

            SetupData();
        }

        /// <summary>
        /// Инициализация списков с тестовыми данными
        /// </summary>
        private void SetupData()
        {
            _dishes = new List<Dish>
    {
        new Dish
        {
            Id = 1,
            Name = "Soup harcho",
            Description = "Very острый, not cool",
            Calories = 200,
            Image = "Images/Soup.jpg",
            Category = _categories.Find(c => c.NormalizedName.Equals("soups")),
            Price = 5.00m 
        },
        new Dish
        {
            Id = 2,
            Name = "Borsch",
            Description = "A lot of salo, without сметаны",
            Calories = 330,
            Image = "Images/Borsch.jpg",
            Category = _categories.Find(c => c.NormalizedName.Equals("soups")),
            Price = 6.50m 
        },
        new Dish
        {
            Id = 3,
            Name = "Krabovyi",
            Description = "Salad with krabovye palochki",
            Calories = 400,
            Image = "Images/Krabovyi.jpg",
            Category = _categories.Find(c => c.NormalizedName.Equals("salads")),
            Price = 7.00m 
        },
        new Dish
        {
            Id = 4,
            Name = "Tea",
            Description = "Hot drink",
            Calories = 120,
            Image = "Images/Tea.jpg",
            Category = _categories.Find(c => c.NormalizedName.Equals("drinks")),
            Price = 2.50m 
        },
        new Dish
        {
            Id = 5,
            Name = "Cake",
            Description = "Cool dessert",
            Calories = 500,
            Image = "Images/Cake.jpg",
            Category = _categories.Find(c => c.NormalizedName.Equals("desserts")),
            Price = 4.00m 
        },
        new Dish
        {
            Id = 6,
            Name = "Chicken soup",
            Description = "Soup like granny's",
            Calories = 500,
            Image = "Images/ChickenSoup.jpg",
            Category = _categories.Find(c => c.NormalizedName.Equals("soups")),
            Price = 5.50m 
        },
        new Dish
        {
            Id = 7,
            Name = "Ramen",
            Description = "Imagine Asia",
            Calories = 500,
            Image = "Images/Ramen.jpg",
            Category = _categories.Find(c => c.NormalizedName.Equals("soups")),
            Price = 8.00m 
        }
    };
        }

        /// <summary>
        /// Получение списка продуктов с фильтрацией по категории и разбиением на страницы
        /// </summary>
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var filteredDishes = _dishes
    .Where(d => string.IsNullOrEmpty(categoryNormalizedName) || categoryNormalizedName == "Все" || d.Category.NormalizedName.Equals(categoryNormalizedName))
    .ToList();

            var totalPages = (int)Math.Ceiling((double)filteredDishes.Count / _itemsPerPage);

            // Убедитесь, что этот код корректно обрабатывает номер страницы
            if (pageNo < 1 || pageNo > totalPages)
            {
                pageNo = 1;
            }


            var paginatedDishes = filteredDishes
                .Skip((pageNo - 1) * _itemsPerPage)  
                .Take(_itemsPerPage) 
                .ToList();

            var result = new ListModel<Dish>
            {
                Items = paginatedDishes,
                CurrentPage = pageNo,
                TotalPages = totalPages 
            };

            return Task.FromResult(ResponseData<ListModel<Dish>>.Success(result));
        }
        public Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var dish = _dishes.FirstOrDefault(d => d.Id == id);

            if (dish == null)
            {
                return Task.FromResult(ResponseData<Dish>.Error("Блюдо не найдено."));
            }

            return Task.FromResult(ResponseData<Dish>.Success(dish));
        }


    }
}
