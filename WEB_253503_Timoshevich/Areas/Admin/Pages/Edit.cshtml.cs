using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_253503_Timoshevich.UI.Services.CategoryService;
using WEB_253503_Timoshevich.UI.Services.FileService; 
using WEB_2535503_Timoshevich.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace WEB_253503_Timoshevich.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService; 

        public EditModel(IProductService productService, ICategoryService categoryService, IFileService fileService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _fileService = fileService; 
        }

        [BindProperty]
        public Dish Dish { get; set; } = new Dish();

        [BindProperty]
        public IFormFile? Upload { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (!response.Successfull || response.Data == null)
            {
                return NotFound();
            }

            Dish = response.Data;

            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoriesResponse.Data, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Upload != null)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/jpg" };
                if (!allowedTypes.Contains(Upload.ContentType))
                {
                    ModelState.AddModelError("Upload", "Неподдерживаемый тип файла. Пожалуйста, загрузите изображение.");
                    return Page();
                }

                var imageUrl = await _fileService.SaveFileAsync(Upload);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    ModelState.AddModelError("", "Не удалось загрузить изображение.");
                    return Page();
                }

                Dish.Image = imageUrl; 
            }

            var response = await _productService.UpdateProductAsync(Dish.Id, Dish, Upload); 
            if (!response.Successfull)
            {
                ModelState.AddModelError("", "Не удалось обновить блюдо: " + response.ErrorMessage);
                return Page();
            }

            return RedirectToPage("./Index");
        }

    }
}