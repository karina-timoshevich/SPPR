using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Timoshevich.API.Data;
using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_2535503_Timoshevich.Domain.Entities;

namespace WEB_253503_Timoshevich.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _context;

        public DeleteModel(IProductService context)
        {
            _context = context;
        }

        [BindProperty]
        public Dish Dish { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var dish = await _context.Dishes.FirstOrDefaultAsync(m => m.Id == id);

            //if (dish == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    Dish = dish;
            //}
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var dish = await _context.Dishes.FindAsync(id);
            //if (dish != null)
            //{
            //    Dish = dish;
            //    _context.Dishes.Remove(Dish);
            //    await _context.SaveChangesAsync();
            //}

            return RedirectToPage("./Index");
        }
    }
}
