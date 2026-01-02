using System.Text.Json.Serialization;

namespace WEB_253551_URBANOVICH.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;

    [JsonIgnore]
    public List<Dish> Dishes { get; set; } = new();
}
