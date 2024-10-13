using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;
using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_253503_Timoshevich.UI.Services.FileService;
using System.Net;
using Microsoft.EntityFrameworkCore;

public class ApiProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly string _pageSize;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiProductService> _logger;

    private readonly IFileService _fileService;

    public ApiProductService(HttpClient httpClient, IConfiguration configuration, IFileService fileService, ILogger<ApiProductService> logger)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Value;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _fileService = fileService; // Внедрение IFileService
        _logger = logger;
    }


    public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}dishes/");

        if (!string.IsNullOrEmpty(categoryNormalizedName))
        {
            urlString.Append($"{categoryNormalizedName}/");
        }

        // Формируем правильный URL для пагинации
        urlString.Append($"?pageNo={pageNo}");

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
        // Первоначально используем картинку по умолчанию
        product.Image = "Images/noimage.jpg";

        // Сохранить файл изображения, если он предоставлен
        if (formFile != null)
        {
            var imageUrl = await _fileService.SaveFileAsync(formFile);
            // Добавить в объект URL изображения
            if (!string.IsNullOrEmpty(imageUrl))
                product.Image = imageUrl;
        }

        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Dishes");
        var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
        }

        _logger.LogError($"-----> объект не создан. Error: {response.StatusCode}");
        return ResponseData<Dish>.Error($"Объект не добавлен. Error: {response.StatusCode}");
    }
    public async Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile = null)
    {
        var existingDishResponse = await GetProductByIdAsync(id);
        if (!existingDishResponse.Successfull)
        {
            return ResponseData<Dish>.Error("Блюдо не найдено");
        }

        if (formFile != null)
        {
            var imageUrl = await _fileService.SaveFileAsync(formFile);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                product.Image = imageUrl;
            }
        }

        // Отправляем обновленные данные блюда на сервер
        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Dishes/{id}");
        var response = await _httpClient.PutAsJsonAsync(uri, product);

        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Возвращаем локально обновленный объект, если сервер возвращает NoContent
                return ResponseData<Dish>.Success(existingDishResponse.Data);
            }

            return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
        }

        return ResponseData<Dish>.Error($"Ошибка обновления: {response.StatusCode}");
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
    public async Task<ResponseData<object>> DeleteProductAsync(int id)
    {
        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Dishes/{id}");
        var response = await _httpClient.DeleteAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            return ResponseData<object>.Success(null); // Успешное удаление
        }

        return ResponseData<object>.Error($"Ошибка удаления: {response.StatusCode}");
    }

}
