using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;
using WEB_253503_Timoshevich.UI.Services.ProductService;

public class ApiProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly string _pageSize;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiProductService> _logger;

    public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Value;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }

    public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}dishes/");

        if (!string.IsNullOrEmpty(categoryNormalizedName))
        {
            urlString.Append($"{categoryNormalizedName}/");
        }

        if (pageNo > 1)
        {
            urlString.Append($"page{pageNo}");
        }

        if (!_pageSize.Equals("3"))
        {
            urlString.Append(QueryString.Create("pageSize", _pageSize));
        }

        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Dish>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return ResponseData<ListModel<Dish>>.Error($"Ошибка: {ex.Message}");
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");
        return ResponseData<ListModel<Dish>>.Error($"Данные не получены от сервера. Error: {response.StatusCode}");
    }

    public async Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
    {
        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Dishes");
        var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
        }

        _logger.LogError($"-----> объект не создан. Error: {response.StatusCode}");
        return ResponseData<Dish>.Error($"Объект не добавлен. Error: {response.StatusCode}");
    }

    public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
    {
        // Получите данные из вашего API
        var urlString = $"{_httpClient.BaseAddress.AbsoluteUri}dishes/{id}";

        var response = await _httpClient.GetAsync(urlString);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>();
        }

        return ResponseData<Dish>.Error("Блюдо не найдено");
    }

}
