using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
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

    #region Endpoints

    [HttpPost()]
    public async Task<IActionResult> AddProduct([FromBody] ProductModel.Request request)
    {
        try
        {
            var product = await _service.AddProduct(request);
            return CreatedAtAction(nameof(GetProductBySku), new { id = product.Id }, product);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (DuplicatedEntityException de)
        {
            return Conflict(de.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al guardar el producto");
        }
    }


    [HttpGet()]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetProducts();
        if (products == null || !products.Any()) return NoContent();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductBySku(Guid id)
    {
        var product = await _service.GetProductById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    #endregion
}
