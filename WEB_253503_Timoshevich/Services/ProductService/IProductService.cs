using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;

namespace WEB_253503_Timoshevich.UI.Services.ProductService
{
    public interface IProductService
    {
       
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
       
        public Task<ResponseData<Dish>> GetProductByIdAsync(int id);

        public Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile);

        Task<ResponseData<object>> DeleteProductAsync(int id);

        public Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile);
    }
}
