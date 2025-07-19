using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Order : EntityBase
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string ShippingAddress { get; set; } = null!;
    public string BillingAddress { get; set; } = null!;
    public string? Notes { get; set; }

    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.PENDING;

    public List<OrderItem> Items { get; set; } = new();
}


