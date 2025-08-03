using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Interfaces;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        if (string.IsNullOrWhiteSpace(request.CustomerId.ToString()) ||
            string.IsNullOrWhiteSpace(request.ShippingAddress) ||
            string.IsNullOrWhiteSpace(request.BillingAddress) 
            // ... queda pendiente la validación para la lista de productos en la orden
            )
        {
            throw new ArgumentException("Valores para la orden no válidos");
        }
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
                //Name = item.Name,
                //Description = item.Description,
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
            Notes = request.Notes, // Se puede ampliar si lo agregás en el Request
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
            order.Notes,
            order.Items.Select(i => new OrderModel.OrderItem(
                i.ProductId,
                i.Quantity,
                //i.Name,
                //i.Description,
                i.UnitPrice
            )).ToList()
        );
    }

    public async Task<List<OrderModel.OrderResponse>> GetOrders(OrderModel.OrderSearchFilter? filter = null)
    {
        //return (List<Product>?) await _repository.GetAll<Product>();

        var orders = await _repository.GetAll<Order>();
        if (orders == null || !orders.Any()) return null;

        return orders.Select(o => new OrderModel.OrderResponse(
            o.Id,
            o.CustomerId,
            o.ShippingAddress,
            o.BillingAddress,
            o.TotalAmount,
            o.Notes,
            o.Items.Select(i => new OrderModel.OrderItem(
                i.ProductId,
                i.Quantity,
                //i.Name,
                //i.Description,
                i.UnitPrice
            )).ToList()
        )).ToList();
    }


    // OPCION 1
    /*public async Task<List<OrderModel.OrderResponse>> GetOrders(OrderModel.OrderSearchFilter filter)
    {
        var query = _repository.Query<Order>();

        if (filter.Status is not null)
            query = query.Where(o => o.Status == filter.Status);

        if (filter.CustomerId is not null)
            query = query.Where(o => o.CustomerId == filter.CustomerId);

        var skip = (filter.PageNumber - 1) * filter.PageSize;

        var orders = await query
            .Skip(skip)
            .Take(filter.PageSize)
            .Include(o => o.Items)
            .ToListAsync();

        return orders.Select(o => new OrderModel.OrderResponse(
            o.Id,
            o.CustomerId,
            o.ShippingAddress,
            o.BillingAddress,
            o.TotalAmount,
            o.Items.Select(i => new OrderModel.OrderItem(
                i.ProductId,
                i.Quantity,
                i.Name,
                i.Description,
                i.UnitPrice
            )).ToList()
        )).ToList();
    }*/

    public async Task<OrderModel.OrderResponse?> GetOrderById(Guid id)
    {
        var order = await _repository.GetById<Order>(id);
        if (order == null) throw new KeyNotFoundException("Orden no encontrada."); 
        return new OrderModel.OrderResponse(
        order.Id,
        order.CustomerId,
        order.ShippingAddress,
        order.BillingAddress,
        order.TotalAmount,
        order.Notes,
        order.Items.Select(i => new OrderModel.OrderItem(
            i.ProductId,
            i.Quantity,
            //i.Name,
            //i.Description,
            i.UnitPrice
        )).ToList()
        );
    }
    
    /*
    public async Task<OrderModel.OrderResponse?> GetOrderById(Guid id)
    {
        var order = await _repository
            .Query<Order>()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null) return null;

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
    }*/

    public async Task<OrderModel.OrderResponse> UpdateOrderStatus(Guid id, string newStatus)
    {
        var order = await _repository.GetById<Order>(id);

        if (order is null)
            throw new KeyNotFoundException($"Orden con ID {id} no encontrada.");

        if (!Enum.TryParse<OrderStatus>(newStatus, true, out var parsedStatus))
            throw new ArgumentException("Estado de orden no válido.");

        order.Status = parsedStatus;
        await _repository.Update(order);

        return new OrderModel.OrderResponse(
            order.Id,
            order.CustomerId,
            order.ShippingAddress,
            order.BillingAddress,
            order.TotalAmount,
            order.Notes,
            order.Items.Select(i => new OrderModel.OrderItem(
                i.ProductId,
                i.Quantity,
                //i.Name,
                //i.Description,
                i.UnitPrice
            )).ToList()
        );
    }

}
