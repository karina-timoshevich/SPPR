using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_253503_Timoshevich.UI.Services.CategoryService;
using WEB_253503_Timoshevich.UI.Services.FileService;
using WEB_253503_Timoshevich.UI.Models;

namespace WEB_253503_Timoshevich.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
           // builder.Services.AddScoped<IProductService, MemoryProductService>();
            var apiUri = builder.Configuration.GetSection("UriData").GetValue<string>("ApiUri");

            // Регистрация HttpClient для IFileService
            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri($"{apiUri}Files"));

        }
    }
}
