using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        var productResponse = await _productService.GetProductListAsync(category, pageNo);
        if (!productResponse.Successfull)
            return NotFound(productResponse.ErrorMessage);

        string currentCategoryName = null;
        var currentCategory = categoriesResponse.Data.Find(cat => cat.NormalizedName == category);
        if (currentCategory != null)
        {
            currentCategoryName = currentCategory.Name;
        }

        ViewData["currentCategoryName"] = string.IsNullOrEmpty(currentCategoryName) ? "Все" : currentCategoryName;
        ViewData["currentCategoryNormalizedName"] = category;

        ViewBag.Categories = categoriesResponse.Data;

        return View(productResponse.Data); 
    }

}
