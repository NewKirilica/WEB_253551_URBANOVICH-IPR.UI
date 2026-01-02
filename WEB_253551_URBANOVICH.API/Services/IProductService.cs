using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;

namespace WEB_253551_URBANOVICH.API.Services;

public interface IProductService
{
    Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
    Task<ResponseData<Dish>> GetProductByIdAsync(int id);
    Task<ResponseData<Dish>> CreateProductAsync(Dish product);
    Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product);
    Task<ResponseData<bool>> DeleteProductAsync(int id);
}
