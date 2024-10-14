using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;

namespace WEB_253503_Timoshevich.API.Services.ProductService
{
    public interface IProductService
    {
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);

        public Task<ResponseData<Dish>> GetProductByIdAsync(int id);

        Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product);

        public Task<ResponseData<object>> DeleteProductAsync(int id);

        public Task<ResponseData<Dish>> CreateProductAsync(Dish product);

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
