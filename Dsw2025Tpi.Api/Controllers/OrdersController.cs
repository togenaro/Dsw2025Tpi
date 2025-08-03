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

    #region Endpoint N°5
    [HttpPost] // Sexto endpoint
    public async Task<IActionResult> CreateOrder([FromBody] OrderModel.OrderRequest request)
    {
        var order = await _service.CreateOrder(request);
        return Created($"api/orders/{order.OrderId}", order);
    }
    #endregion

    #region Endpoint N°7
    [HttpGet]
    public async Task<IActionResult> GetOrders(
                                               /*OrderStatus? status,
                                               Guid? customerId,
                                               int pageNumber = 1,
                                               int pageSize = 10*/
                                               )
    {
        //var filter = new OrderModel.OrderSearchFilter(/*status, customerId, pageNumber, pageSize*/);
        var result = await _service.GetOrders();

        if (result is null || result.Count == 0) return NoContent();

        return Ok(result);
    }

    /*public async Task<IActionResult> GetOrders(
        [FromQuery] OrderStatus? status,
        [FromQuery] Guid? customerId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var filter = new OrderModel.OrderSearchFilter(status, customerId, pageNumber, pageSize);
        var result = await _service.GetOrders(filter);

        if (result is null || result.Count == 0)
            return NoContent();

        return Ok(result);
    }*/
    #endregion

    #region Endpoint N°8
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var result = await _service.GetOrderById(id);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
    #endregion

    #region Endpoint N°9
    [HttpPut("{id}/status")] // Noveno endpoint
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderModel.OrderStatusUpdate request)
    {
        var result = await _service.UpdateOrderStatus(id, request.NewStatus);
        return Ok(result);
    }
    #endregion

}

