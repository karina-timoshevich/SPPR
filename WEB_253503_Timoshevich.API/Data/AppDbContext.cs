using Microsoft.EntityFrameworkCore;
using WEB_2535503_Timoshevich.Domain.Entities; 

namespace WEB_253503_Timoshevich.API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
    }
}
