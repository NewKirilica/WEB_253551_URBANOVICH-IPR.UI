using Microsoft.EntityFrameworkCore;
using WEB_253551_URBANOVICH.API.Data;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;

namespace WEB_253551_URBANOVICH.API.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext context) => _context = context;

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var data = await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
        return ResponseData<List<Category>>.Success(data);
    }
}
