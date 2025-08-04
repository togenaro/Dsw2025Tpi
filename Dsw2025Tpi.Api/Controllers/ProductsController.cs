using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/products")]
public class ProductsController : ControllerBase
{

    #region Inyección de los servicios
    private readonly ProductsManagementService _service;

    public ProductsController(ProductsManagementService service)
    {
        _service = service;
    }
    #endregion

    #region Endpoint N°1
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [HttpPost()] 
    public async Task<IActionResult> AddProduct([FromBody] ProductModel.ProductRequest request) 
    {
        var product = await _service.AddProduct(request);
        return Created($"api/products/{product.Id}", product);
    }
    #endregion

    #region Endpoint N°2
    [AllowAnonymous]
    [HttpGet()] 
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetProducts();
        if (products is null) return NoContent(); // 204
        return Ok(products); // 200
    }
    #endregion

    #region Endpoint N°3
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _service.GetProductById(id);
        if (product == null) return NotFound(); // 404
        return Ok(product); // 200
    }
    #endregion

    #region Endpoint N°4
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductModel.ProductRequest request)
    {
        var result = await _service.UpdateProduct(id, request);
        return Ok(result); // 200 OK con el producto actualizado
    }
    #endregion

    #region Endpoint N°5
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [HttpPatch("{id}")]
    public async Task<IActionResult> InactivateProduct(Guid id)
    {
        await _service.InactivateProduct(id);
        return NoContent(); // 204
    }
    #endregion

}
