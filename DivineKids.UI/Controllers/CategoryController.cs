using DivineKids.Application.Features.Categories;
using DivineKids.Application.Features.Categories.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivineKids.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    private readonly ILogger<CategoryController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [AllowAnonymous]
    [HttpGet("GetAllAsync")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving categories.");
            return StatusCode(500, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpGet("GetAsync/{id:int}", Name = "GetCategoryById")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);
            if (category is null) return NotFound();
            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving a category.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("AddAsync")]
    public async Task<IActionResult> CreateAsync(CreateCategoryCommand categoryCommand, CancellationToken cancellationToken)
    {
        if (categoryCommand is null)
        {
            return BadRequest("Category cannot be null.");
        }
        try
        {
            var result = await _categoryService.CreateAsync(categoryCommand, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a category.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("UpdateAsync/{id:int}", Name = "UpdateCategoryAsync")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateCategoryCommand categoryCommand, CancellationToken cancellationToken)
    {
        if (categoryCommand is null)
        {
            return BadRequest("Category cannot be null.");
        }
        try
        {
            var result = await _categoryService.UpdateAsync(id, categoryCommand, cancellationToken);
            if (result is null) return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating a category.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("DeleteAsync/{id:int}", Name = "CategoryDeleteAsync")]
    public async Task<IActionResult> CategoryDeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _categoryService.DeleteAsync(id, cancellationToken);
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
