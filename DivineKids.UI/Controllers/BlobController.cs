using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace DivineKids.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public partial class BlobController : ControllerBase
{
    private static readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
    };

    private const long MaxFileBytes = 5 * 1024 * 1024; // 5 MB

    [HttpPost("UploadImage")]
    [RequestSizeLimit(MaxFileBytes)]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromServices] IWebHostEnvironment environment, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file uploaded.");

        if (file.Length > MaxFileBytes)
            return BadRequest($"File exceeds size limit of {MaxFileBytes / (1024 * 1024)} MB.");

        var ext = Path.GetExtension(file.FileName);
        if (!_allowedExtensions.Contains(ext))
            return BadRequest("Unsupported file type.");

        // Ensure wwwroot exists (normally auto if folder exists in project)
        if (string.IsNullOrWhiteSpace(environment.WebRootPath))
        {
            environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var relativeFolder = Path.Combine("uploads", "images");
        var physicalFolder = Path.Combine(environment.WebRootPath, relativeFolder);
        Directory.CreateDirectory(physicalFolder);

        // Sanitize original name (optional - not stored, but helpful if you decide to use it)
        var baseName = Path.GetFileNameWithoutExtension(file.FileName);
        baseName = SanitizeFileNameRegex().Replace(baseName, "-");
        if (string.IsNullOrWhiteSpace(baseName))
            baseName = "img";

        var safeName = $"{baseName}-{Guid.NewGuid():N}{ext}".ToLowerInvariant();
        var fullPath = Path.Combine(physicalFolder, safeName);

        await using (var stream = System.IO.File.Create(fullPath))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        var relativeUrl = Path.Combine(Path.DirectorySeparatorChar.ToString(), relativeFolder, safeName)
            .Replace(Path.DirectorySeparatorChar, '/');
        var absoluteUrl = $"{Request.Scheme}://{Request.Host}{relativeUrl}";

        return Ok(new
        {
            fileName = safeName,
            url = relativeUrl,
            absoluteUrl,
            size = file.Length,
            contentType = file.ContentType
        });
    }

    private static Regex SanitizeFileNameRegex()
    {
        return new Regex(@"[^a-zA-Z0-9_-]+", RegexOptions.Compiled);
    }
}