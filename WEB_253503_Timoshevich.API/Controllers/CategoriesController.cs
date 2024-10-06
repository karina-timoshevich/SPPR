using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEB_253503_Timoshevich.API.Services.CategoryService;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;

namespace WEB_253503_Timoshevich.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
        {
            var response = await _categoryService.GetCategoryListAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<string>>> GetCategory(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);
            if (!response.Successfull)
            {
                return NotFound(response.ErrorMessage);
            }
            return Ok(response);
        }


        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<ResponseData<Category>>> PostCategory(Category category)
        {
            var response = await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = response.Data.Id }, response);
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);
            if (!response.Successfull)
            {
                return NotFound(response.ErrorMessage);
            }

            return NoContent();
        }
    }
}
