using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Domain.Entities;

public class Product : EntityBase
{
    public Product()
    {

    }
    public Product(string sku, string name, decimal price)
    {
        Sku = sku;
        Name = name;
        CurrentUnitPrice = price;
        IsActive = true;
    }
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public decimal CurrentUnitPrice { get; set; }
    public bool IsActive { get; set; }

    public Guid? CategoryId { get; set; }

    // public Category? Category { get; set; }

}
