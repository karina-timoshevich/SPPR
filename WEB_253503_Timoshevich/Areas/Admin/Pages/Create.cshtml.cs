using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_253503_Timoshevich.UI.Services.CategoryService; // Не забудьте этот using
using WEB_2535503_Timoshevich.Domain.Entities;
using System.Net.Http.Headers;

namespace WEB_253503_Timoshevich.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IProductService productService, ICategoryService categoryService, IHttpClientFactory httpClientFactory)
        {
            _productService = productService;
            _categoryService = categoryService;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Dish Dish { get; set; } = new Dish();

        [BindProperty]
        public IFormFile? Image { get; set; } // Используйте это свойство для файла изображения

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
            if (!ModelState.IsValid) // Проверяем валидность модели
            {
                return Page(); // Возвращаем страницу с ошибками
            }

            // Проверяем, было ли загружено изображение
            if (Image != null)
            {
                // Загружаем изображение и получаем URL
                var imageUrl = await UploadImageToApiAsync(Image);
                if (string.IsNullOrEmpty(imageUrl)) // Если загрузка не удалась
                {
                    ModelState.AddModelError("", "Не удалось загрузить изображение."); // Добавляем ошибку
                    return Page(); // Возвращаем страницу с ошибками
                }

                // Устанавливаем URL изображения для объекта Dish
                Dish.Image = imageUrl;
            }

            // Создаем новый объект с использованием сервиса
            var response = await _productService.CreateProductAsync(Dish, Image);
            if (response.Successfull) // Если создание прошло успешно
            {
                return RedirectToPage("./Index"); // Перенаправляем на страницу индекса
            }

            // Если возникла ошибка при создании
            ModelState.AddModelError("", "Не удалось создать блюдо: " + response.ErrorMessage);
            return Page(); // Возвращаем страницу с ошибками
        }
        private async Task<string> UploadImageToApiAsync(IFormFile imageFile)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = "https://localhost:7002/api/files"; // Укажите правильный URL API

            using var content = new MultipartFormDataContent();
            using var fileStreamContent = new StreamContent(imageFile.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
            content.Add(fileStreamContent, "file", imageFile.FileName);

            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var imageUrl = await response.Content.ReadAsStringAsync();
                return imageUrl.Trim('"'); // Возвращаем URL изображения
            }

            return string.Empty; // Если произошла ошибка, возвращаем пустую строку
        }

    }
}
