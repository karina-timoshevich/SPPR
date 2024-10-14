using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_253503_Timoshevich.UI.Services.CategoryService;
using WEB_253503_Timoshevich.UI.Services.FileService;
using WEB_2535503_Timoshevich.Domain.Entities;
using System.Net.Http.Headers;

namespace WEB_253503_Timoshevich.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IFileService _fileService;

        public CreateModel(IProductService productService, ICategoryService categoryService, IHttpClientFactory httpClientFactory, IFileService fileService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _httpClientFactory = httpClientFactory;
            _fileService = fileService;
        }

        [BindProperty]
        public Dish Dish { get; set; } = new Dish();

        [BindProperty]
        public IFormFile? Image { get; set; } 

        public async Task<IActionResult> OnGetAsync()
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (categoriesResponse.Successfull)
            {
                ViewData["CategoryId"] = new SelectList(categoriesResponse.Data, "Id", "Name");
            }
            else
            {
                ModelState.AddModelError("", categoriesResponse.ErrorMessage);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) 
            {
                return Page(); 
            }

            if (Image != null)
            {
               // var imageUrl = await UploadImageToApiAsync(Image);
                var imageUrl = await _fileService.SaveFileAsync(Image);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    ModelState.AddModelError("", "Не удалось загрузить изображение.");
                    return Page(); 
                }
                Dish.Image = imageUrl;
            }

            var response = await _productService.CreateProductAsync(Dish, Image);
            if (response.Successfull) 
            {
                return RedirectToPage("./Index"); 
            }

            ModelState.AddModelError("", "Не удалось создать блюдо: " + response.ErrorMessage);
            return Page();
        }
        private async Task<string> UploadImageToApiAsync(IFormFile imageFile)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = "https://localhost:7002/api/files";

            using var content = new MultipartFormDataContent();
            using var fileStreamContent = new StreamContent(imageFile.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
            content.Add(fileStreamContent, "file", imageFile.FileName);
            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var imageUrl = await response.Content.ReadAsStringAsync();
                return imageUrl.Trim('"'); 
            }

            return string.Empty; 
        }

    }
}
