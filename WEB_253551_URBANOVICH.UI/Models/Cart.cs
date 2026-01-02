using System.Text.Json.Serialization;

namespace WEB_253551_URBANOVICH.UI.Models;

public class CartItem
{
    public int DishId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = "Images/noimage.jpg";
    public int Quantity { get; set; } = 1;

    [JsonIgnore]
    public decimal LineTotal => Price * Quantity;
}

public class Cart
{
    public List<CartItem> Items { get; set; } = new();

    public int ItemsCount => Items.Sum(i => i.Quantity);
    public decimal TotalPrice => Items.Sum(i => i.LineTotal);

    public void Add(int dishId, string name, decimal price, string image, int qty = 1)
    {
        var item = Items.FirstOrDefault(i => i.DishId == dishId);
        if (item == null)
        {
            Items.Add(new CartItem
            {
                DishId = dishId,
                Name = name,
                Price = price,
                Image = image,
                Quantity = qty
            });
        }
        else
        {
            item.Quantity += qty;
        }
    }

    public void RemoveOne(int dishId)
    {
        var item = Items.FirstOrDefault(i => i.DishId == dishId);
        if (item == null) return;
        item.Quantity--;
        if (item.Quantity <= 0) Items.Remove(item);
    }

    public void RemoveItem(int dishId)
    {
        var item = Items.FirstOrDefault(i => i.DishId == dishId);
        if (item != null) Items.Remove(item);
    }

    public void Clear() => Items.Clear();
}

public class CartViewModel
{
    public decimal TotalPrice { get; set; }
    public int ItemsCount { get; set; }
}
