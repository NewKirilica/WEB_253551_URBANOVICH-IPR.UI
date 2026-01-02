using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253551_URBANOVICH.API.Services;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;

namespace WEB_253551_URBANOVICH.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "admin")]
public class DishesController : ControllerBase
{
    private readonly IProductService _service;
    public DishesController(IProductService service) => _service = service;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ResponseData<ListModel<Dish>>> GetList(
        [FromQuery] string? category,
        [FromQuery] int pageNo = 1,
        [FromQuery] int pageSize = 3)
        => await _service.GetProductListAsync(category, pageNo, pageSize);

    [HttpGet("byid/{id:int}")]
    [AllowAnonymous]
    public async Task<ResponseData<Dish>> GetById(int id)
        => await _service.GetProductByIdAsync(id);

    [HttpPost]
    public async Task<ResponseData<Dish>> Create([FromBody] Dish dish)
        => await _service.CreateProductAsync(dish);

    [HttpPut("{id:int}")]
    public async Task<ResponseData<Dish>> Update(int id, [FromBody] Dish dish)
    => await _service.UpdateProductAsync(id, dish);

    [HttpDelete("{id:int}")]
    public async Task<ResponseData<bool>> Delete(int id)
        => await _service.DeleteProductAsync(id);
}
