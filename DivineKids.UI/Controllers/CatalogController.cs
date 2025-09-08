using DivineKids.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DivineKids.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("GetCatalogTreeAsync")]
    public async Task<IActionResult> GetCatalogTreeAsync(CancellationToken cancellationToken)
    {
        try
        {
            var catalogTree = await _catalogService.GetCatalogTreeAsync(cancellationToken);
            return Ok(catalogTree);
        }
        catch (Exception ex)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, "Internal server error");
        }
    }

}
