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

    [HttpPost()] // Primer endpoint
    public async Task<IActionResult> AddProduct([FromBody] ProductModel.ProductRequest request)
    {
        try
        {
            var product = await _service.AddProduct(request);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            //return Ok(product);
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


    [HttpGet()] // Segundo endpoint
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetProducts();
        if (products == null || !products.Any()) return NoContent(); // 204
        return Ok(products); // 200
    }


    [HttpGet("{id}")] // Tercer endpoint
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _service.GetProductById(id);
        if (product == null) return NotFound(); // 404
        return Ok(product); // 20
    }


    [HttpPut("{id}")] // Cuarto endpoint
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductModel.ProductUpdate request)
    {
        try
        {
            var result = await _service.UpdateProduct(id, request);
            return Ok(result); // 200 OK con el producto actualizado
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message); // 404
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message); // 400
        }
        catch (Exception)
        {
            return Problem("Error al actualizar el producto"); // 500
        }
    }


    [HttpPatch("{id}")] // Quinto endpoint
    // Como no hay cuerpo de solicitud(PATCH sin payload),
    // no hace falta crear un DTO específico para esto.
    public async Task<IActionResult> InactivateProduct(Guid id)
    {
        try
        {
            await _service.InactivateProduct(id);
            return NoContent(); // 204
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message); // 404
        }
        catch (Exception)
        {
            return Problem("Error al intentar inhabilitar el producto"); // 500
        }
    }

    #endregion
}
