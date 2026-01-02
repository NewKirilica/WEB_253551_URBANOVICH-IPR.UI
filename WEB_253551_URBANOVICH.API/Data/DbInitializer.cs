using Microsoft.EntityFrameworkCore;
using WEB_253551_URBANOVICH.Domain.Entities;

namespace WEB_253551_URBANOVICH.API.Data;

public static class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.EnsureCreatedAsync();

        if (await context.Categories.AnyAsync() || await context.Dishes.AnyAsync())
            return;

        var apiHost = app.Configuration.GetSection("UriData:ApiHost").Value ?? "https://localhost:7002";
        string Img(string file) => $"{apiHost}/Images/{file}";

        var categories = new List<Category>
        {
            new() { Name = "Супы", NormalizedName = "soups" },
            new() { Name = "Горячее", NormalizedName = "hot-dishes" },
            new() { Name = "Десерты", NormalizedName = "desserts" },
            new() { Name = "Напитки", NormalizedName = "drinks" }
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var dishes = new List<Dish>
        {
            new() { Name="Борщ", Description="Классический борщ со сметаной.", Price=18, CategoryId=categories[0].Id, Image=Img("noimage.jpg") },
            new() { Name="Том-ям", Description="Острый суп с креветками и кокосовым молоком.", Price=24, CategoryId=categories[0].Id, Image=Img("noimage.jpg") },
            new() { Name="Стейк", Description="Говяжий стейк средней прожарки.", Price=70, CategoryId=categories[1].Id, Image=Img("noimage.jpg") },
            new() { Name="Паста", Description="Паста с соусом болоньезе.", Price=30, CategoryId=categories[1].Id, Image=Img("noimage.jpg") },
            new() { Name="Чизкейк", Description="Нежный чизкейк Нью-Йорк.", Price=32, CategoryId=categories[2].Id, Image=Img("noimage.jpg") },
            new() { Name="Мороженое", Description="Пломбир с топпингом.", Price=9, CategoryId=categories[2].Id, Image=Img("noimage.jpg") },
            new() { Name="Лимонад", Description="Домашний лимонад.", Price=9, CategoryId=categories[3].Id, Image=Img("noimage.jpg") },
            new() { Name="Кофе", Description="Американо.", Price=9, CategoryId=categories[3].Id, Image=Img("noimage.jpg") },
        };

        context.Dishes.AddRange(dishes);
        await context.SaveChangesAsync();
    }
}
