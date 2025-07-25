using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System.Linq.Expressions;
using System.Text.Json;

namespace Dsw2025Tpi.Data.Repositories;

public class InMemoryRepository : IRepository
{
private List<Product>? _products;
private List<Customer>? _customers;
private List<Order>? _orders;

    public InMemoryRepository()
    {
        LoadProducts();
    }

    private List<T> GetList<T>() where T : EntityBase
    {
        if (typeof(T) == typeof(Product)) return _products as List<T> ?? new List<T>();
        if (typeof(T) == typeof(Customer)) return _customers as List<T> ?? new List<T>();
        if (typeof(T) == typeof(Order)) return _orders as List<T> ?? new List<T>();
        throw new NotSupportedException($"Tipo {typeof(T).Name} no soportado en InMemory.");
    }

    private void SetList<T>(List<T> list) where T : EntityBase
    {
        if (typeof(T) == typeof(Product)) _products = list as List<Product>;
        else throw new NotSupportedException($"Tipo {typeof(T).Name} no soportado en InMemory.");
    }

    public async Task<T?> GetById<T>(Guid id, params string[] include) where T : EntityBase
    {
        return await Task.FromResult(GetList<T>().FirstOrDefault(e => e.Id == id));
    }

    public async Task<IEnumerable<T>?> GetAll<T>(params string[] include) where T : EntityBase
    {
        return await Task.FromResult(GetList<T>());
    }

    public async Task<T?> First<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
    {
        return await Task.FromResult(GetList<T>().FirstOrDefault(predicate.Compile()));
    }

    public async Task<IEnumerable<T>?> GetFiltered<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
    {
        return await Task.FromResult(GetList<T>().Where(predicate.Compile()));
    }

    public async Task<T> Add<T>(T entity) where T : EntityBase
    {
        var list = GetList<T>();
        list.Add(entity);
        SetList(list);
        return await Task.FromResult(entity);
    }

    public async Task<T> Update<T>(T entity) where T : EntityBase
    {
        var list = GetList<T>();
        var index = list.FindIndex(e => e.Id == entity.Id);
        if (index >= 0)
        {
            list[index] = entity;
            SetList(list);
        }
        return await Task.FromResult(entity);
    }

    public async Task<T> Delete<T>(T entity) where T : EntityBase
    {
        var list = GetList<T>();
        list.RemoveAll(e => e.Id == entity.Id);
        SetList(list);
        return await Task.FromResult(entity);
    }

    public IQueryable<T> Query<T>() where T : EntityBase
    {
        return GetList<T>().AsQueryable();
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


