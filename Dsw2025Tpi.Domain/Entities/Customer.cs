using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Customer : EntityBase
{
    #region Relaciones con otras entidades
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    #endregion

    #region Propiedades propias de la entidad
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    #endregion

    #region Constructor por defecto necesario para EF Core
    public Customer()
    {
    }
    public Customer(string name, string email, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }
    #endregion
}
