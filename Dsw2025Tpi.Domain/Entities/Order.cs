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
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }
    public string? Notes { get; set; }

    public decimal TotalAmount => Items.Sum(i => i.Subtotal);
    public OrderStatus Status { get; set; }
    #endregion

    #region Constructores
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

}


