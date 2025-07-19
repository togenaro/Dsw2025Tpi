using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Interfaces;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Services;

public class OrderManagementService
{
    private readonly IRepository _repository;
    public OrderManagementService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderModel.OrderResponse> CreateOrder(OrderModel.OrderRequest request)
    {
        // Verificar existencia del cliente
        var customer = await _repository.GetById<Customer>(request.CustomerId);
        if (customer == null)
            throw new ArgumentException($"Cliente con ID {request.CustomerId} no encontrado.");

        decimal total = 0m;
        var orderItems = new List<OrderItem>();

        foreach (var item in request.OrderItems)
        {
            var product = await _repository.GetById<Product>(item.ProductId);

            if (product == null)
                throw new ArgumentException($"Producto con ID {item.ProductId} no encontrado.");

            if (product.StockQuantity < item.Quantity)
                throw new ArgumentException($"Stock insuficiente para el producto {product.Name}.");

            product.StockQuantity -= item.Quantity;
            await _repository.Update(product);

            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                Name = item.Name,
                Description = item.Description,
                UnitPrice = item.CurrentUnitPrice,
                Quantity = item.Quantity
            };

            orderItems.Add(orderItem);
            total += orderItem.Subtotal;
        }

        var order = new Order
        {
            CustomerId = request.CustomerId,
            Customer = customer,
            ShippingAddress = request.ShippingAddress,
            BillingAddress = request.BillingAddress,
            Notes = null, // Se puede ampliar si lo agregás en el Request
            Date = DateTime.UtcNow,
            Status = OrderStatus.PENDING,
            TotalAmount = total,
            Items = orderItems
        };

        await _repository.Add(order);

        return new OrderModel.OrderResponse(
            order.Id,
            order.CustomerId,
            order.ShippingAddress,
            order.BillingAddress,
            order.TotalAmount,
            order.Items.Select(i => new OrderModel.OrderItem(
                i.ProductId,
                i.Quantity,
                i.Name,
                i.Description,
                i.UnitPrice
            )).ToList()
        );
    }


}
