using WEB_253503_Timoshevich.API.Data;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace WEB_253503_Timoshevich.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var product = await _context.Dishes.FindAsync(id);
            if (product == null)
            {
                return ResponseData<Dish>.Error("Product not found");
            }
            return ResponseData<Dish>.Success(product);
        }

        public async Task UpdateProductAsync(int id, Dish product)
        {
            var existingProduct = await _context.Dishes.FindAsync(id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<object>> DeleteProductAsync(int id)
        {
            var product = await _context.Dishes.FindAsync(id);
            if (product == null)
            {
                return ResponseData<object>.Error("Product not found");
            }

            _context.Dishes.Remove(product);
            await _context.SaveChangesAsync();

            return ResponseData<object>.Success(null);  // Успешное удаление
        }


        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product)
        {
            _context.Dishes.Add(product);
            await _context.SaveChangesAsync();
            return ResponseData<Dish>.Success(product);
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            // Здесь должен быть код для сохранения изображения и возвращения URL
            return ResponseData<string>.Success("Image saved successfully");
        }

        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(
        string? categoryNormalizedName,
        int pageNo = 1,
        int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var query = _context.Dishes.AsQueryable();
            var dataList = new ListModel<Dish>();
            query = query
            .Where(d => categoryNormalizedName == null
            ||
            d.Category.NormalizedName.Equals(categoryNormalizedName));
            // количество элементов в списке
            var count = await query.CountAsync(); //.Count();
            if (count == 0)
            {
                return ResponseData<ListModel<Dish>>.Success(dataList);
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<Dish>>.Error("No such page");
            dataList.Items = await query
            .OrderBy(d => d.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            return ResponseData<ListModel<Dish>>.Success(dataList);
        }
    }


}
