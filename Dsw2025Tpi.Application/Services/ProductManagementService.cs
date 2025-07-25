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

    public async Task<ProductModel.ProductResponse> AddProduct(ProductModel.ProductRequest request)
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

        return new ProductModel.ProductResponse
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

    public async Task<List<ProductModel.ProductResponse>?> GetProducts()
    {
        //return (List<Product>?) await _repository.GetAll<Product>();

        var products = await _repository.GetAll<Product>();
        if (products == null || !products.Any()) return null;
        return products
            .Where(p => p.IsActive)
            .Select(p => new ProductModel.ProductResponse
        (
            p.Id,
            p.Sku!,
            p.InternalCode!,
            p.Name!,
            p.Description!,
            p.CurrentUnitPrice,
            p.StockQuantity,
            p.IsActive
        )).ToList();
    }

    public async Task<ProductModel.ProductResponse?> GetProductById(Guid id)
    {
        //return await _repository.GetById<Product>(id);
        var product = await _repository.GetById<Product>(id);
        if (product == null) return null;
        return new ProductModel.ProductResponse
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

    public async Task<ProductModel.ProductResponse> UpdateProduct(Guid id, ProductModel.ProductUpdate update)
    {
        var product = await _repository.GetById<Product>(id);

        if (product == null)
            throw new KeyNotFoundException("Producto no encontrado.");

        // Validaciones básicas
        if (string.IsNullOrWhiteSpace(update.Sku) ||
            string.IsNullOrWhiteSpace(update.Name) ||
            string.IsNullOrWhiteSpace(update.InternalCode) ||
            update.CurrentUnitPrice <= 0 ||
            update.StockQuantity < 0)
        {
            throw new ArgumentException("Datos inválidos para actualizar el producto.");
        }

        // Asignación de campos
        product.Sku = update.Sku;
        product.InternalCode = update.InternalCode;
        product.Name = update.Name;
        product.Description = update.Description;
        product.CurrentUnitPrice = update.CurrentUnitPrice;
        product.StockQuantity = update.StockQuantity;

        await _repository.Update(product);

        return new ProductModel.ProductResponse(
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

    public async Task InactivateProduct(Guid id)
    {
        var product = await _repository.GetById<Product>(id);

        if (product == null)
            throw new KeyNotFoundException("Producto no encontrado.");

        product.IsActive = false;

        await _repository.Update(product);
    }

}
