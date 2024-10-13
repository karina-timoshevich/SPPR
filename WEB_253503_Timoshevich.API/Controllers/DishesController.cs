﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEB_253503_Timoshevich.API.Services.ProductService;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;

namespace WEB_253503_Timoshevich.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IProductService _productService;

        public DishesController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Dishes/main-dishes?pageno=2
        [HttpGet("{category}")]
        public async Task<ActionResult<ResponseData<List<Dish>>>> GetDishes(string category, int pageNo = 1, int pageSize = 3)
        {
            var response = await _productService.GetProductListAsync(category, pageNo, pageSize);
            return Ok(response);
        }

        // GET: api/Dishes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseData<Dish>>> GetDish(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Successfull)
            {
                return NotFound(response.ErrorMessage);
            }
            return Ok(response);
        }

        // POST: api/Dishes
        [HttpPost]
        public async Task<ActionResult<ResponseData<Dish>>> PostDish(Dish dish)
        {
            var response = await _productService.CreateProductAsync(dish);
            return CreatedAtAction(nameof(GetDish), new { id = response.Data.Id }, response);
        }
        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Dish>>>> GetAllDishes(int pageNo = 1, int pageSize = 3)
        {
            var response = await _productService.GetProductListAsync(null, pageNo, pageSize);
            return Ok(response);
        }


        // DELETE: api/Dishes/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (!response.Successfull)
            {
                return NotFound(response.ErrorMessage);
            }

            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDish(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest("ID блюда не совпадает с ID в теле запроса.");
            }

            var response = await _productService.UpdateProductAsync(id, dish);
            if (!response.Successfull)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data); // Возвращаем обновленный объект вместо NoContent
        }
    }
}
