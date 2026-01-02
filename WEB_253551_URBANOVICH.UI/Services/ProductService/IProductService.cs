using Microsoft.AspNetCore.Http;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;

namespace WEB_253551_URBANOVICH.UI.Services.ProductService;

public interface IProductService
{
    Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
    Task<ResponseData<Dish>> GetProductByIdAsync(int id);
    Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile);
    Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile);
    Task DeleteProductAsync(int id);
}
