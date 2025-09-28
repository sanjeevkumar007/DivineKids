using DivineKids.Application.Contracts;
using DivineKids.Application.Features.MainCategories;
using DivineKids.Application.Features.MainCategories.Commands;
using DivineKids.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivineKids.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MainCategoryController(IMainCategoryService mainCategoryService, ILogger<MainCategoryController> logger) : ControllerBase
{
    private readonly IMainCategoryService _mainCategoryService = mainCategoryService ?? throw new ArgumentNullException(nameof(mainCategoryService));
    private readonly ILogger<MainCategoryController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpGet("GetAllAsync")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _mainCategoryService.GetAllAsync(cancellationToken);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving main categories.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("GetAsync/{id:int}", Name = "GetMainCategoryById")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _mainCategoryService.GetByIdAsync(id, cancellationToken);
            if (category is null) return NotFound();
            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving a main category.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("AddAsync")]
    public async Task<IActionResult> CreateAsync(CreateMainCategoryCommand categoryCommand, CancellationToken cancellationToken)
    {
        if (categoryCommand is null)
        {
            return BadRequest("Category cannot be null.");
        }
        try
        {
            var result = await _mainCategoryService.CreateAsync(categoryCommand, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a main category.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("UpdateAsync/{id:int}", Name = "UpdateMainCategoryAsync")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateMainCategoryCommand category, CancellationToken cancellationToken)
    {
        if (category is null)
        {
            return BadRequest("Category cannot be null.");
        }
        try
        {
            await _mainCategoryService.UpdateAsync(id, category, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating a main category.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("DeleteAsync/{id:int}", Name = "DeleteMainCategoryAsync")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _mainCategoryService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting a main category.");
            return StatusCode(500, "Internal server error");
        }
    }
}
