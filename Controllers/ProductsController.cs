using Microsoft.AspNetCore.Mvc;
using ABM_Productos.Models.DTOs.Requests;
using ABM_Productos.Services.Interfaces;

namespace ABM_Productos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        return Ok(_service.GetAllProducts());
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = _service.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("search")]
    public IActionResult SearchProductsByName([FromQuery] string name)
    {
        var products = _service.SearchProductsByName(name ?? string.Empty);
        return Ok(products);
    }

    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var stats = _service.GetStats();
        return Ok(stats);
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] ProductForCreateDto dto)
    {
        var createdProduct = _service.CreateProduct(dto);
        if (createdProduct == null)
        {
            return Conflict(new { message = "Ya existe un producto con el mismo nombre." });
        }

        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] ProductForUpdateDto dto)
    {
        var existingProduct = _service.GetProductById(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        _service.UpdateProduct(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var existingProduct = _service.GetProductById(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        _service.DeleteProduct(id);
        return NoContent();
    }
}

