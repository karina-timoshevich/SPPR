using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Timoshevich.Data;
using WEB_253503_Timoshevich.UI.Extensions;
using WEB_253503_Timoshevich.UI.Services.ProductService;
using WEB_253503_Timoshevich.UI.Services.CategoryService;
using WEB_253503_Timoshevich.UI.Models;

var builder = WebApplication.CreateBuilder(args);
// Получение UriData из конфигурации
var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();
builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));


builder.Services.AddControllersWithViews();
builder.RegisterCustomServices();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
