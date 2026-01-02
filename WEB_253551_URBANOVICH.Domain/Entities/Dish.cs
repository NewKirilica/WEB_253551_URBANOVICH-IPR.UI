namespace WEB_253551_URBANOVICH.Domain.Entities;

public class Dish
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Image { get; set; } = "Images/noimage.jpg";

    public string? MimeType { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
