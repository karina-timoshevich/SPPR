using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WEB_2535503_Timoshevich.Domain.Entities;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WEB_253503_Timoshevich.API.Data;

public static class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        if (context.Categories.Any() || context.Dishes.Any())
            return;

        var config = app.Configuration;
        var baseUrl = config["AppSettings:BaseUrl"];

        var categories = new Category[]
        {
            new Category {Id=1, Name="Стартеры", NormalizedName="starters"},
            new Category {Id=2, Name="Салаты", NormalizedName="salads"},
            new Category {Id=3, Name="Супы", NormalizedName="soups"},
            new Category {Id=4, Name="Основные блюда", NormalizedName="main-dishes"},
            new Category {Id=5, Name="Напитки", NormalizedName="drinks"},
            new Category {Id=6, Name="Десерты", NormalizedName="desserts"}
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var dishes = new Dish[]
        {
            new Dish { Name = "Soup harcho", Description = "Very острый, not cool", Calories = 200, Image = $"{baseUrl}/Images/Soup.jpg", CategoryId = categories.First(c => c.NormalizedName == "soups").Id },
            new Dish { Name = "Borsch", Description = "A lot of salo, without сметаны", Calories = 330, Image = $"{baseUrl}/Images/Borsch.jpg", CategoryId = categories.First(c => c.NormalizedName == "soups").Id },
            new Dish { Name = "Krabovyi", Description = "Salad with krabovye palochki", Calories = 400, Image = $"{baseUrl}/Images/Krabovyi.jpg", CategoryId = categories.First(c => c.NormalizedName == "salads").Id },
            new Dish { Name = "Tea", Description = "Hot drink", Calories = 120, Image = $"{baseUrl}/Images/Tea.jpg", CategoryId = categories.First(c => c.NormalizedName == "drinks").Id },
            new Dish { Name = "Cake", Description = "Cool dessert", Calories = 500, Image = $"{baseUrl}/Images/Cake.jpg", CategoryId = categories.First(c => c.NormalizedName == "desserts").Id },
            new Dish { Name = "Chicken soup", Description = "Soup kak y granny", Calories = 500, Image = $"{baseUrl}/Images/ChickenSoup.jpg", CategoryId = categories.First(c => c.NormalizedName == "soups").Id },
            new Dish { Name = "Ramen", Description = "Imagine Asia", Calories = 500, Image = $"{baseUrl}/Images/Ramen.jpg", CategoryId = categories.First(c => c.NormalizedName == "soups").Id }
        };

        context.Dishes.AddRange(dishes);
        await context.SaveChangesAsync();
    }
}
