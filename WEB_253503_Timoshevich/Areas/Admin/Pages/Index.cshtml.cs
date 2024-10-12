using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_253503_Timoshevich.UI.Services.ProductService;

namespace WEB_253503_Timoshevich.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Dish> Dish { get; set; } = new List<Dish>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int? pageNo)
        {
            CurrentPage = pageNo ?? 1;

            var response = await _productService.GetProductListAsync(null, CurrentPage);
            if (response.Successfull)
            {
                Dish = response.Data.Items;
                TotalPages = response.Data.TotalPages;
            }
            else
            {
                Dish = new List<Dish>();
            }
        }
    }

}
