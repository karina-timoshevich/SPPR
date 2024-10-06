using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Timoshevich.API.Data;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;

namespace WEB_253503_Timoshevich.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(categories);
        }

        public async Task<ResponseData<Category>> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return ResponseData<Category>.Error("Category not found");
            }
            return ResponseData<Category>.Success(category);
        }

        public async Task<ResponseData<Category>> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return ResponseData<Category>.Success(category);
        }

        public async Task<ResponseData<string>> UpdateCategoryAsync(int id, Category category)
        {
            if (id != category.Id)
            {
                return ResponseData<string>.Error("Invalid category ID");
            }

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return ResponseData<string>.Success("Category updated successfully");
        }

        public async Task<ResponseData<string>> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return ResponseData<string>.Error("Category not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return ResponseData<string>.Success("Category deleted successfully");
        }
    }
}
