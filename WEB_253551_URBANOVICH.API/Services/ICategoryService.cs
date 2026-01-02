using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;

namespace WEB_253551_URBANOVICH.API.Services;

public interface ICategoryService
{
    Task<ResponseData<List<Category>>> GetCategoryListAsync();
}
