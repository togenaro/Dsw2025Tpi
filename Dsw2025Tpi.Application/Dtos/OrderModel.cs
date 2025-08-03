using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Dtos;

public record OrderModel
{
    public record OrderItem(
        Guid ProductId,
        int Quantity,
        decimal CurrentUnitPrice,
        decimal subtotal  
    );

    public record OrderRequest(
        Guid CustomerId,
        string ShippingAddress,
        string BillingAddress,
        string? Notes,
        List<OrderItem> OrderItems
    );

    public record OrderResponse(
        Guid OrderId,
        Guid CustomerId,
        string ShippingAddress,
        string BillingAddress,
        decimal TotalAmount,
        string? Notes,
        string status,
        List<OrderItem> OrderItems
    );

    public record OrderSearchFilter(
        OrderStatus? Status,
        Guid? CustomerId,
        int PageNumber = 1,
        int PageSize = 10
    );

    public record OrderStatusUpdate(Guid OrderId, string NewStatus);
}
