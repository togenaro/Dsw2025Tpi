using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Order : EntityBase
{
    #region Propiedades de las relaciones
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    #endregion

    #region Propiedades propias de la entidad
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? ShippingAddress { get; set; }
    public string? BillingAddress { get; set; }
    public string? Notes { get; set; }

    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.PENDING;
    #endregion

    #region Constructor por defecto necesario para EF Core
    public Order()
    {
    }
    public Order(Guid customerId, string? shippingAddress, string? billingAddress, string? notes)
    {
        CustomerId = customerId;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Notes = notes;
    }
    #endregion

    // No es necesario incluir la propiedad Products, ya que los productos se manejan a través de OrderItems.

    // Del ejercicio 15:
    // public ICollection<Product> Products { get; } = new HashSet<Product>(); 
    // public List<OrderItem> Items { get; set; } = new();


}


