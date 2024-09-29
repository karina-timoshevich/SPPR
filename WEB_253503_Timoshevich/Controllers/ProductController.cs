using Microsoft.AspNetCore.Mvc;
using WEB_253503_Timoshevich.UI.Services.CategoryService;
using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
        var categoriesResponse = await _categoryService.GetCategoryListAsync();
        if (!categoriesResponse.Successfull)
            return NotFound(categoriesResponse.ErrorMessage);

        // Передаем данные в продуктовый сервис
        var productResponse = await _productService.GetProductListAsync(category, pageNo);
        if (!productResponse.Successfull)
            return NotFound(productResponse.ErrorMessage);

        // Передаем данные в представление
        ViewData["currentCategory"] = string.IsNullOrEmpty(category) ? "Все" : category;
        ViewBag.Categories = categoriesResponse.Data;

        return View(productResponse.Data); // Возвращаем правильную модель
    }

}
