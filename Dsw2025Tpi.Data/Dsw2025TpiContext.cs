using Microsoft.EntityFrameworkCore;
using Dsw2025Tpi.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext: DbContext
{
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Products
        modelBuilder.Entity<Product>(eb =>
        {
            // No es necesario plantear la clave primaria para las entidades, ya que Entity Framework
            // la crea automáticamente al detectar que la entidad posee una propiedad cuyo nombre contiene
            // "Id" o "Guid". 
            // Por ejemplo, en este caso, la propiedad "Id" de la clase "EntityBase" es detectada como clave primaria.
            eb.ToTable("Products");
            eb.Property(p => p.Sku)
              .HasMaxLength(20)
              .IsRequired();
            eb.Property(p => p.Name)
              .HasMaxLength(100)
              .IsRequired();
            eb.Property(p => p.InternalCode)
              .IsRequired()
              .HasMaxLength(45);
            eb.Property(p => p.Description)
              .IsRequired()
              .HasMaxLength(100);
            eb.Property(p => p.CurrentUnitPrice)
              .IsRequired()
              .HasPrecision(15,2); // 15 digitos en total, de los cuales 2 son decimales.  
            eb.Property(p => p.StockQuantity)
              .IsRequired()
              .HasDefaultValue(0);
            eb.Property(p => p.IsActive)
               .IsRequired()
               .HasDefaultValue(true);
        });
        #endregion

        #region Orders
        modelBuilder.Entity<Order>(eb =>
        {
            eb.ToTable("Orders");
            eb.Property(o => o.Date)
              .HasDefaultValueSql("GETUTCDATE()") // Establece la fecha por defecto como la fecha y hora actual en UTC. 
              .IsRequired();
            eb.Property(o => o.ShippingAddress)
              .HasMaxLength(45);
            eb.Property(o => o.BillingAddress)
              .HasMaxLength(45);
            eb.Property(o => o.Notes)
              .HasMaxLength(200);
            eb.Property(o => o.Status)
              .HasDefaultValue(OrderStatus.PENDING) // Establece el estado por defecto como PENDING.
              .IsRequired();
            eb.Ignore(o => o.TotalAmount); // Ignora la propiedad TotalAmount, ya que es calculada y no se almacena en la base de datos.
        });
        #endregion

        #region OrderItems
        modelBuilder.Entity<OrderItem>(eb =>
        {
            eb.ToTable("OrderItems");
            eb.Property(oi => oi.UnitPrice)
              .HasPrecision(15, 2) // 15 digitos en total, de los cuales 2 son decimales.
              .IsRequired();
            eb.Property(oi => oi.Quantity)
              .IsRequired()
              .HasDefaultValue(0);
            eb.Ignore(oi => oi.Subtotal); // Ignora la propiedad Subtotal, ya que es calculada y no se almacena en la base de datos.
        });
        #endregion

        #region Customers
        modelBuilder.Entity<Customer>(eb =>
        {
            eb.ToTable("Customers");
            eb.Property(c => c.Name)
              .HasMaxLength(45)
              .IsRequired();
            eb.Property(c => c.Email)
              .HasMaxLength(45)
              .IsRequired();
            eb.Property(c => c.PhoneNumber)
              .HasMaxLength(30);
        });
        #endregion
    }

    /*HasDefaultValue():
        Se utiliza para especificar un valor constante como valor predeterminado para una propiedad.
        Por ejemplo, HasDefaultValue(true) establecerá el valor predeterminado de una propiedad booleana como true.
        El valor se aplica directamente al insertar una nueva entidad en la base de datos.
    HasDefaultValueSql(): 
        Se utiliza para especificar una expresión SQL que se evalúa en la base de datos para determinar 
        el valor predeterminado.*/

}
