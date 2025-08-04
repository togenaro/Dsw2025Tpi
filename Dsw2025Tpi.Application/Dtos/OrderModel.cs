using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Dtos;

public record OrderModel
{
    public record OrderItemResponse(
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal
    );

    public record OrderItemRequest(
        Guid ProductId,
        int Quantity
    );

    public record OrderRequest(
        Guid CustomerId,
        string ShippingAddress,
        string BillingAddress,
        string? Notes,
        List<OrderItemRequest> OrderItems
    );

    public record OrderResponse(
        Guid OrderId,
        Guid CustomerId,
        string ShippingAddress,
        string BillingAddress,
        decimal TotalAmount,
        string? Notes,
        string status,
        List<OrderItemResponse> OrderItems
    );

    public record OrderSearchFilter(
        OrderStatus? Status,
        Guid? CustomerId,
        int PageNumber = 1,
        int PageSize = 10
    );

    public record OrderStatusUpdate(Guid OrderId, string NewStatus);
}
