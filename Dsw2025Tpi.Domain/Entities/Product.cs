using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Domain.Entities;

public class Product : EntityBase
{
    public Product()
    {

    }
    public Product(string sku, string internalCode, string name, string description, decimal price, int stock)
    {
        Sku = sku;
        InternalCode = internalCode;
        Name = name;
        Description = description;
        CurrentUnitPrice = price;
        StockQuantity = stock;
        IsActive = true;
    }
    public string? Sku { get; set; }
    public string? InternalCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal CurrentUnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }


    // public Guid? CategoryId { get; set; }

    // public Category? Category { get; set; }


}
