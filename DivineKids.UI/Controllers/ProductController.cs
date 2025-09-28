using DivineKids.Application.Features.Prodoucts;
using DivineKids.Application.Features.Prodoucts.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivineKids.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController(IProductService productService, ILogger<ProductController> logger) : ControllerBase
{
    private readonly IProductService _productService = productService;
    private readonly ILogger<ProductController> _logger = logger;

    [AllowAnonymous]
    [HttpGet("GetAllAsync")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productService.GetAllProductsAsync(cancellationToken);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving main products.");
            return StatusCode(500, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpGet("GetAsync/{id}", Name = "GetByProductIdAsync")]
    public async Task<IActionResult> GetByProductIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving a main category.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("AddAsync")]
    public async Task<IActionResult> CreateProductAsync(CreateProductCommand productCommand, CancellationToken cancellationToken)
    {
        if (productCommand is null)
        {
            return BadRequest("Category cannot be null.");
        }
        try
        {
            var result = await _productService.CreateProductAsync(productCommand, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a main category.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("UpdateAsync/{id}", Name = "UpdateProductAsync")]
    public async Task<IActionResult> UpdateProductAsync(int id, UpdateProductCommand productCommand, CancellationToken cancellationToken)
    {
        if (productCommand is null)
        {
            return BadRequest("Category cannot be null.");
        }

        try
        {
            var result = await _productService.UpdateProductAsync(id, productCommand, cancellationToken);
            if (result is null) return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating a category.");
            return StatusCode(500, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpGet("GetByCategoryIdAsync/{categoryId}", Name = "GetByCategoryIdAsync")]
    public async Task<IActionResult> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productService.GetAllProductsAsync(cancellationToken);
            var filteredProducts = products.Where(p => p.CategoryId == categoryId).ToList();
            if (filteredProducts.Count == 0)
            {
                return NotFound();
            }
            return Ok(filteredProducts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving products by category ID.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("DeleteAsync/{id}", Name = "DeleteProductAsync")]
    public async Task<IActionResult> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _productService.DeleteProductAsync(id, cancellationToken);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting a category.");
            return StatusCode(500, "Internal server error");
        }
    }
}
