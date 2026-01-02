using Microsoft.AspNetCore.Mvc;
using WEB_253551_URBANOVICH.API.Services;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;

namespace WEB_253551_URBANOVICH.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;
    public CategoriesController(ICategoryService service) => _service = service;

    [HttpGet]
    public async Task<ResponseData<List<Category>>> Get() =>
        await _service.GetCategoryListAsync();
}
