using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WEB_253503_Timoshevich.API.Data;
using WEB_253503_Timoshevich.API.Models;
using WEB_253503_Timoshevich.API.Services.CategoryService;
using WEB_253503_Timoshevich.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();


var authServer = builder.Configuration
.GetSection("AuthServer")
.Get<AuthServerData>();
// Добавить сервис аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
    // Адрес метаданных конфигурации OpenID
    o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
    // Authority сервера аутентификации
    o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
    // Audience для токена JWT
    o.Audience = "account";
    // Запретить HTTPS для использования локальной версии Keycloak
    // В рабочем проекте должно быть true
    o.RequireHttpsMetadata = false;
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
await DbInitializer.SeedData(app);
app.Run();
