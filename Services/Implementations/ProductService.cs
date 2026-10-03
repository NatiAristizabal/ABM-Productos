using ABM_Productos.Entities;
using ABM_Productos.Models.DTOs.Requests;
using ABM_Productos.Models.DTOs.Responses;
using ABM_Productos.Repositories.Interfaces;
using ABM_Productos.Services.Interfaces;

namespace ABM_Productos.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public List<ProductForReadDto> GetAllProducts()
    {
        var products = _repository.GetAllProducts();
        var dtos = new List<ProductForReadDto>();

        foreach (var product in products)
        {
            dtos.Add(new ProductForReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });
        }

        return dtos;
    }

    public ProductForReadDto? GetProductById(int id)
    {
        var product = _repository.GetProductById(id);
        if (product == null) return null;

        return new ProductForReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public ProductForReadDto? CreateProduct(ProductForCreateDto dto)
    {
        var products = _repository.GetAllProducts();
        bool exists = products.Any(p => p.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));
        if (exists) return null;

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price
        };

        _repository.AddProduct(product);

        return new ProductForReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public void UpdateProduct(int id, ProductForUpdateDto dto)
    {
        var existingProduct = _repository.GetProductById(id);
        if (existingProduct != null)
        {
            existingProduct.Name = dto.Name;
            existingProduct.Price = dto.Price;
            _repository.UpdateProduct(existingProduct);
        }
    }

    public void DeleteProduct(int id)
    {
        var product = _repository.GetProductById(id);
        if (product != null)
        {
            _repository.DeleteProduct(product);
        }
    }

    public List<ProductForReadDto> SearchProductsByName(string name)
    {
        var products = _repository.SearchProductsByName(name);
        var dtos = new List<ProductForReadDto>();

        foreach (var product in products)
        {
            dtos.Add(new ProductForReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });
        }

        return dtos;
    }

    public ProductStatsDto GetStats()
    {
        var products = _repository.GetAllProducts();

        if (!products.Any())
        {
            return new ProductStatsDto
            {
                Total = 0,
                AveragePrice = 0m,
                MostExpensiveName = string.Empty
            };
        }

        return new ProductStatsDto
        {
            Total = products.Count,
            AveragePrice = products.Average(p => p.Price),
            MostExpensiveName = products.OrderByDescending(p => p.Price).First().Name
        };
    }
}
