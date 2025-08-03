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
using Azure.Core;

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
        #region Validación de datos de la request
        if (string.IsNullOrWhiteSpace(request.CustomerId.ToString()) ||
            string.IsNullOrWhiteSpace(request.ShippingAddress) ||
            string.IsNullOrWhiteSpace(request.BillingAddress) ||
            string.IsNullOrWhiteSpace(request.OrderItems.ToString())
            // ... queda pendiente la validación para la lista de productos en la orden
            )
        {
            throw new ArgumentException("Valores para la orden no válidos");
        }
        #endregion

        #region Validación de existencia del cliente
        var customer = await _repository.GetById<Customer>(request.CustomerId);
        if (customer == null)
            throw new ArgumentException($"Cliente con ID {request.CustomerId} no encontrado.");
        #endregion

        var orderItems = new List<OrderItem>();

        foreach (var item in request.OrderItems)
        {
            #region Validación y modificación de productos en DB, según la request
            var product = await _repository.GetById<Product>(item.ProductId);

            // Validación
            if (product == null)
                throw new ArgumentException($"Producto con ID {item.ProductId} no encontrado.");
            if (product.StockQuantity < item.Quantity)
                throw new ArgumentException($"Stock insuficiente para el producto {product.Name}.");

            // Modificación en DB
            product.StockQuantity -= item.Quantity;
            await _repository.Update(product);
            #endregion

            #region Adición del item en la lista de items de la orden
            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            };

            orderItems.Add(orderItem);
            #endregion
        }

        #region Adición de la orden en DB
        var order = new Order
        {
            CustomerId = request.CustomerId,
            Customer = customer,
            ShippingAddress = request.ShippingAddress,
            BillingAddress = request.BillingAddress,
            Notes = request.Notes, 
            Date = DateTime.UtcNow,
            Status = OrderStatus.PENDING,
            Items = orderItems
        };
        await _repository.Add(order);
        #endregion

        #region Response Body de la orden creada
        return new OrderModel.OrderResponse(
            order.Id,
            order.CustomerId,
            order.ShippingAddress,
            order.BillingAddress,
            order.TotalAmount,
            order.Notes!,
            order.Status.ToString(),
            order.Items.Select(i => new OrderModel.OrderItemResponse(
                i.ProductId,
                i.Quantity,
                i.UnitPrice,
                i.Subtotal
            )).ToList()
        );
        #endregion
    }

    public async Task<List<OrderModel.OrderResponse>> GetOrders(OrderModel.OrderSearchFilter? filter = null)
    {
        var orders = await _repository.GetAll<Order>();
        var orderItems = await _repository.GetAll<OrderItem>();
        if (orders == null || !orders.Any()) return null!;

        foreach(var order in orders)
        {
            order.Items = orderItems!.Where(i => i.OrderId == order.Id).ToList();
        }

        return orders.Select(o => new OrderModel.OrderResponse(
            o.Id,
            o.CustomerId,
            o.ShippingAddress,
            o.BillingAddress,
            o.TotalAmount,
            o.Notes!,
            o.Status.ToString(),
            o.Items.Select(i => new OrderModel.OrderItemResponse
            (
                i.ProductId,
                i.Quantity,
                i.UnitPrice,
                i.Subtotal
            )).ToList()
        )).ToList();
    }

    public async Task<OrderModel.OrderResponse?> GetOrderById(Guid id)
    {
        var order = await _repository.GetById<Order>(id);
        var orderItems = await _repository.GetAll<OrderItem>();
        if (order == null) throw new KeyNotFoundException("Orden no encontrada.");

        foreach(var items in orderItems!)
        {
            order.Items = orderItems.Where(i => i.OrderId == order.Id).ToList();
        }

        return new OrderModel.OrderResponse(
        order.Id,
        order.CustomerId,
        order.ShippingAddress,
        order.BillingAddress,
        order.TotalAmount,
        order.Notes!,
        order.Status.ToString(),
        order.Items.Select(i => new OrderModel.OrderItemResponse
            (
                i.ProductId,
                i.Quantity,
                i.UnitPrice,
                i.Subtotal
            )).ToList()
        );
    }
    

    public async Task<OrderModel.OrderResponse> UpdateOrderStatus(Guid id, string newStatus)
    {
        var order = await _repository.GetById<Order>(id);

        if (order is null)
            throw new KeyNotFoundException($"Orden con ID {id} no encontrada.");

        if (!Enum.TryParse<OrderStatus>(newStatus, true, out var parsedStatus))
            throw new ArgumentException("Estado de orden no válido.");

        order.Status = parsedStatus;
        await _repository.Update(order);

        var orderItems = await _repository.GetAll<OrderItem>();
        foreach (var items in orderItems!)
        {
            order.Items = orderItems.Where(i => i.OrderId == order.Id).ToList();
        }

        return new OrderModel.OrderResponse(
            order.Id,
            order.CustomerId,
            order.ShippingAddress,
            order.BillingAddress,
            order.TotalAmount,
            order.Notes!,
            order.Status.ToString(),
            order.Items.Select(i => new OrderModel.OrderItemResponse
            (
                i.ProductId,
                i.Quantity,
                i.UnitPrice,
                i.Subtotal
            )).ToList()
        );
    }

}
