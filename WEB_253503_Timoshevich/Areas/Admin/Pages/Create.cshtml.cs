using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_Timoshevich.API.Data;
using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_2535503_Timoshevich.Domain.Entities;

namespace WEB_253503_Timoshevich.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _context;

        public CreateModel(IProductService context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Dish Dish { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Dishes.Add(Dish);
            //await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
