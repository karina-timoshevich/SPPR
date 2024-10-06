using System.Collections.Generic;
using System.Threading.Tasks;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;

namespace WEB_253503_Timoshevich.API.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ResponseData<List<Category>>> GetCategoryListAsync();
        Task<ResponseData<Category>> GetCategoryByIdAsync(int id);
        Task<ResponseData<Category>> CreateCategoryAsync(Category category);
        Task<ResponseData<string>> UpdateCategoryAsync(int id, Category category);
        Task<ResponseData<string>> DeleteCategoryAsync(int id);
    }
}
