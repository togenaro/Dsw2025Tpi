namespace Dsw2025Tpi.Application.Dtos;

public record OrderModel
{
    public record OrderItem(
        Guid ProductId,
        int Quantity,
        string Name,
        string Description,
        decimal CurrentUnitPrice
    );

    public record OrderRequest(
        Guid CustomerId,
        string ShippingAddress,
        string BillingAddress,
        List<OrderItem> OrderItems
    );

    public record OrderResponse(
        Guid OrderId,
        Guid CustomerId,
        string ShippingAddress,
        string BillingAddress,
        decimal TotalAmount,
        List<OrderItem> OrderItems
    );
}
