using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;

namespace WEB_253503_Timoshevich.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiCategoryService> _logger;

        public ApiCategoryService(HttpClient httpClient, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var response = await _httpClient.GetAsync("categories");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");
            return ResponseData<List<Category>>.Error($"Данные не получены от сервера. Error: {response.StatusCode}");
        }

    }

}
