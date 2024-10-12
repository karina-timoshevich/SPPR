using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Timoshevich.API.Data;
using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_2535503_Timoshevich.Domain.Entities;


namespace WEB_253503_Timoshevich.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }

        public Dish Dish { get; set; } = default!; // Убедитесь, что Dish инициализировано

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value); // Используйте ваш метод API для получения блюда
            if (!response.Successfull || response.Data == null)
            {
                return NotFound();
            }

            Dish = response.Data; // Инициализация свойства Dish

            return Page();
        }
    }
}

