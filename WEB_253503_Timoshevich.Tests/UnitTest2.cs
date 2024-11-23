using Microsoft.EntityFrameworkCore;
using WEB_253503_Timoshevich.API.Data;
using WEB_253503_Timoshevich.API.Services.ProductService;
using WEB_2535503_Timoshevich.Domain.Entities;
using WEB_2535503_Timoshevich.Domain.Models;
using Xunit;

public class ProductServiceTests
{
    private readonly AppDbContext _dbContext;

    public ProductServiceTests()
    {
        _dbContext = CreateInMemoryDbContext();
    }

    private ProductService CreateProductService()
    {
        return new ProductService(_dbContext);
    }

    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var categories = new List<Category>
        {
            new Category { Name = "Супы", NormalizedName = "soups" },
            new Category { Name = "Салаты", NormalizedName = "salads" }
        };

        context.Categories.AddRange(categories);

        for (int i = 0; i < 30; i++)
        {
            var dish = new Dish
            {
                Name = "Dish" + i,
                Description = "Description" + i,
                Price = 100 + i,
                Calories = 200 + i,
                Category = categories[i % 2]
            };
            context.Dishes.Add(dish);
        }

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task ServiceReturnsFirstPageOfThreeItems()
    {
        var service = CreateProductService();
        var result = await service.GetProductListAsync(null);

        Assert.IsType<ResponseData<ListModel<Dish>>>(result);
        Assert.True(result.Successfull);
        Assert.Equal(1, result.Data.CurrentPage);
        Assert.Equal(3, result.Data.Items.Count);
        Assert.Equal(10, result.Data.TotalPages);
        Assert.Equal(_dbContext.Dishes.First(), result.Data.Items[0]);

        _dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task ServiceReturnsCorrectPage()
    {
        var service = CreateProductService();
        var result = await service.GetProductListAsync(null, 2);

        Assert.IsType<ResponseData<ListModel<Dish>>>(result);
        Assert.True(result.Successfull);
        Assert.Equal(2, result.Data.CurrentPage);
        Assert.Equal(3, result.Data.Items.Count);
        Assert.Equal(10, result.Data.TotalPages);

        _dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task ServiceReturnsCorrectFilteredData()
    {
        var service = CreateProductService();
        var result = await service.GetProductListAsync("soups");

        Assert.IsType<ResponseData<ListModel<Dish>>>(result);
        Assert.True(result.Successfull);

        var firstDishOfCategory = _dbContext.Dishes.First(d => d.Category.NormalizedName == "soups");
        Assert.Equal(firstDishOfCategory, result.Data.Items[0]);

        _dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task ServiceLimitsPageSizeToMaximum()
    {
        var service = CreateProductService();
        var result = await service.GetProductListAsync(null, 1, 40);

        Assert.IsType<ResponseData<ListModel<Dish>>>(result);
        Assert.True(result.Successfull);
        Assert.Equal(20, result.Data.Items.Count); // Проверка ограничения размера страницы

        _dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task ServiceReturnsErrorWhenPageNumberExceedsTotalPages()
    {
        var service = CreateProductService();
        var result = await service.GetProductListAsync("soups", 15);

        Assert.False(result.Successfull);
        Assert.Equal("No such page", result.ErrorMessage);

        _dbContext.Database.EnsureDeleted();
    }
}
