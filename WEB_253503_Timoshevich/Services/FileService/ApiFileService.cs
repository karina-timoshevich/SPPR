using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using WEB_253503_Timoshevich.UI.Services.Authentication;

namespace WEB_253503_Timoshevich.UI.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private ITokenAccessor _tokenAccessor;

        public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor;
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            var urlString = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}");
            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);

            content.Add(streamContent, "file", newName);

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.PostAsync(urlString, content);
            if (response.IsSuccessStatusCode)
            {
                // Вернуть полученный Url сохраненного файла
                return await response.Content.ReadAsStringAsync();
            }
            return String.Empty;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.DeleteAsync(fileName);
        }
    }





    //public class ApiFileService : IFileService
    //{
    //    private readonly HttpClient _httpClient;
    //    private ITokenAccessor _tokenAccessor;


    //    public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
    //    {
    //        _httpClient = httpClient;
    //        _tokenAccessor = tokenAccessor;
    //    }

    //    public async Task<string> SaveFileAsync(IFormFile formFile)
    //    {
    //        if (formFile == null)
    //        {
    //            throw new ArgumentNullException(nameof(formFile));
    //        }

    //        var extension = Path.GetExtension(formFile.FileName);
    //        var newFileName = $"{Guid.NewGuid()}{extension}";

    //        var uploadPath = Path.Combine("wwwroot", "Images");
    //        if (!Directory.Exists(uploadPath))
    //        {
    //            Directory.CreateDirectory(uploadPath);
    //        }

    //        var filePath = Path.Combine(uploadPath, newFileName);
    //        using (var stream = new FileStream(filePath, FileMode.Create))
    //        {
    //            await formFile.CopyToAsync(stream);
    //        }
    //        return $"/Images/{newFileName}";
    //    }

    //    public async Task DeleteFileAsync(string fileName)
    //    {
    //        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
    //        var response = await _httpClient.DeleteAsync($"Files/{fileName}");
    //        if (!response.IsSuccessStatusCode)
    //        {
    //            throw new Exception("Ошибка при удалении файла");
    //        }
    //    }
    //}


}
