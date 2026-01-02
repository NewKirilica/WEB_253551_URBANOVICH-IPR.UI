namespace WEB_253551_URBANOVICH.Domain.Models;

public class ListModel<T>
{
    public IList<T> Items { get; set; } = new List<T>();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
}
