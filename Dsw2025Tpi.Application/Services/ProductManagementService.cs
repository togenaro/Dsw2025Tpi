using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Interfaces;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Services;

public class ProductsManagementService
{
    private readonly IRepository _repository;

    public ProductsManagementService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        return await _repository.GetById<Product>(id);
    }

    public async Task<List<Product>?> GetProducts()
    {
        return (List<Product>?)await _repository.GetAll<Product>();
    }

    public async Task<ProductModel.Response> AddProduct(ProductModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Sku) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.InternalCode) ||
            request.CurrentUnitPrice <= 0 ||
            request.StockQuantity < 0
            )
        {
            throw new ArgumentException("Valores para el producto no válidos");
        }

        var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
        if (exist != null) throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");

        /*var product = new Product(request.Sku, request.Name, request.CurrentUnitPrice);
        await _repository.Add(product);
        return new ProductModel.Response(product.Id);*/

        var product = new Product
        (
            request.Sku,
            request.InternalCode,
            request.Name,
            request.Description,
            request.CurrentUnitPrice,
            request.StockQuantity
        );

        await _repository.Add(product);

        return new ProductModel.Response
        (
            product.Id,
            product.Sku!,
            product.InternalCode!,
            product.Name!,
            product.Description!,
            product.CurrentUnitPrice,
            product.StockQuantity,
            product.IsActive
        );
    }
}
