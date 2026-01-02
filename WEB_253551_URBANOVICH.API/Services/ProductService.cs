using Microsoft.EntityFrameworkCore;
using WEB_253551_URBANOVICH.API.Data;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;

namespace WEB_253551_URBANOVICH.API.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly int _maxPageSize = 20;

    public ProductService(AppDbContext context) => _context = context;

    public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > _maxPageSize)
            pageSize = _maxPageSize;

        var query = _context.Dishes
            .Include(d => d.Category)
            .AsQueryable();

        query = query.Where(d => string.IsNullOrEmpty(categoryNormalizedName) ||
                          d.Category.NormalizedName == categoryNormalizedName);


        var dataList = new ListModel<Dish>();

        var count = await query.CountAsync();
        if (count == 0)
            return ResponseData<ListModel<Dish>>.Success(dataList);

        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        if (pageNo > totalPages || pageNo < 1)
            return ResponseData<ListModel<Dish>>.Error("No such page");

        dataList.Items = await query
            .OrderBy(d => d.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        dataList.CurrentPage = pageNo;
        dataList.TotalPages = totalPages;

        return ResponseData<ListModel<Dish>>.Success(dataList);
    }

    public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
    {
        var dish = await _context.Dishes
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == id);

        return dish == null
            ? ResponseData<Dish>.Error("Not found")
            : ResponseData<Dish>.Success(dish);
    }

    public async Task<ResponseData<Dish>> CreateProductAsync(Dish product)
    {
        product.Category = null!;
        _context.Dishes.Add(product);
        await _context.SaveChangesAsync();
        return await GetProductByIdAsync(product.Id);
    }

    public async Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product)
    {
        var existing = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        if (existing == null)
            return ResponseData<Dish>.Error("Not found");

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Image = product.Image;
        existing.MimeType = product.MimeType;
        existing.CategoryId = product.CategoryId;

        await _context.SaveChangesAsync();
        return await GetProductByIdAsync(existing.Id);
    }

    public async Task<ResponseData<bool>> DeleteProductAsync(int id)
    {
        var existing = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        if (existing == null)
            return ResponseData<bool>.Error("Not found");

        _context.Dishes.Remove(existing);
        await _context.SaveChangesAsync();
        return ResponseData<bool>.Success(true);
    }
}
