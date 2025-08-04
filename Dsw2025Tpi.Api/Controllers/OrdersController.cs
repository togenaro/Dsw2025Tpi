using Dsw2025Tpi.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public class OrdersController : ControllerBase
{

    #region Inyección del servicio

    private readonly OrderManagementService _service;

    public OrdersController(OrderManagementService service)
    {
        _service = service;
    }

    #endregion

    #region Endpoint N°6
    [Authorize(Roles = "User")]
    [HttpPost] 
    public async Task<IActionResult> CreateOrder([FromBody] OrderModel.OrderRequest request)
    {
        var order = await _service.CreateOrder(request);
        return Created($"api/orders/{order.OrderId}", order);
    }
    #endregion

    #region Endpoint N°7
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _service.GetOrders();
        if (result is null) return NoContent();

        return Ok(result);
    }
    #endregion

    #region Endpoint N°8
    [Authorize(Roles = "Admin,User")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var result = await _service.GetOrderById(id);

        if (result is null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region Endpoint N°9
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderModel.OrderStatusUpdate request)
    {
        var result = await _service.UpdateOrderStatus(id, request.NewStatus);
        return Ok(result);
    }
    #endregion

}

