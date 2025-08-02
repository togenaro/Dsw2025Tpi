using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class OrderItem : EntityBase
{
    #region Relaciones con otras entidades
    public Guid? ProductId { get; set; } // FK
    public Product? Product { get; set; } // Navigation property

    public Guid? OrderId { get; set; } // FK
    public Order? Order { get; set; } // Navigation property
    #endregion

    #region Propiedades propias de la entidad
    //public string? Name { get; set; }
    //public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => UnitPrice * Quantity;
    #endregion

    #region Constructor por defecto necesario para EF Core (no se si va)
    public OrderItem()
    {
    }
    public OrderItem(Guid productId, Guid orderId, string name, string description, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        OrderId = orderId;
        //Name = name;
        //Description = description;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
    #endregion
}
