using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System.Linq.Expressions;
using System.Text.Json;

namespace Dsw2025Tpi.Data.Repositories;

public class InMemoryRepository : IRepository
{
    private List<Product>? _products;

    public InMemoryRepository()
    {
        LoadProducts();
    }

    public Task<Product> Add<Product>(Product entity) where Product : EntityBase
    {
        if (entity is Domain.Entities.Product p)
        {
            _products ??= new List<Domain.Entities.Product>();
            _products.Add(p);
            return Task.FromResult(entity);
        }

        throw new InvalidOperationException("Solo se admite la entidad Product en este repositorio.");
    }

    public Task<Product> Delete<Product>(Product entity) where Product : EntityBase
    {
        if (entity is Domain.Entities.Product p)
        {
            _products?.RemoveAll(x => x.Id == p.Id);
            return Task.FromResult(entity);
        }

        throw new InvalidOperationException("Solo se admite la entidad Product en este repositorio.");
    }

    public Task<Product?> First<Product>(Expression<Func<Product, bool>> predicate, params string[] include) where Product : EntityBase
    {
        if (typeof(Product) == typeof(Domain.Entities.Product))
        {
            var compiled = predicate.Compile();
            var match = _products?.Cast<Product>().FirstOrDefault(compiled);
            return Task.FromResult(match);
        }

        throw new InvalidOperationException("Solo se admite la entidad Product en este repositorio.");
    }

    public Task<IEnumerable<Product>?> GetAll<Product>(params string[] include) where Product : EntityBase
    {
        if (typeof(Product) == typeof(Domain.Entities.Product))
        {
            return Task.FromResult<IEnumerable<Product>?>(_products?.Cast<Product>().ToList());
        }

        throw new InvalidOperationException("Solo se admite la entidad Product en este repositorio.");
    }

    public Task<Product?> GetById<Product>(Guid id, params string[] include) where Product : EntityBase
    {
        if (typeof(Product) == typeof(Domain.Entities.Product))
        {
            var match = _products?.Cast<Product>().FirstOrDefault(p => p.Id == id);
            return Task.FromResult(match);
        }

        throw new InvalidOperationException("Solo se admite la entidad Product en este repositorio.");
    }

    public Task<IEnumerable<Product>?> GetFiltered<Product>(Expression<Func<Product, bool>> predicate, params string[] include) where Product : EntityBase
    {
        if (typeof(Product) == typeof(Domain.Entities.Product))
        {
            var result = _products?.Cast<Product>().Where(predicate.Compile()).ToList();
            return Task.FromResult<IEnumerable<Product>?>(result);
        }

        throw new InvalidOperationException("Solo se admite la entidad Product en este repositorio.");
    }

    public IQueryable<Product> Query<Product>() where Product : EntityBase
    {
        if (typeof(Product) == typeof(Domain.Entities.Product))
        {
            return _products?.Cast<Product>().AsQueryable() ?? Enumerable.Empty<Product>().AsQueryable();
        }

        throw new InvalidOperationException("Solo se admite la entidad Product en este repositorio.");
    }

    public Task<Product> Update<Product>(Product entity) where Product : EntityBase
    {
        if (entity is Domain.Entities.Product updated)
        {
            var index = _products?.FindIndex(x => x.Id == updated.Id);
            if (index != null && index >= 0)
            {
                _products![index.Value] = updated;
            }

            return Task.FromResult(entity);
        }

        throw new InvalidOperationException("Solo se admite la entidad Product en este repositorio.");
    }

    public void LoadProducts()
    {
        var json = File.ReadAllText("..//Dsw2025Tpi.Data/Sources/products.json");
        _products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}


