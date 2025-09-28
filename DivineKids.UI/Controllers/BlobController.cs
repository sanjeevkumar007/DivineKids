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
    public async Task<IActionResult> UploadImage(
    IFormFile file,
    [FromServices] IWebHostEnvironment env,
    CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        if (file.Length > MaxFileBytes)
            return BadRequest($"File exceeds size limit of {MaxFileBytes / (1024 * 1024)} MB.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(ext))
            return BadRequest("Unsupported file type.");

        // Ensure WebRootPath is set (wwwroot)
        var webRoot = env.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
            webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        // Ensure uploads folder exists
        var uploadFolder = Path.Combine(webRoot, "uploads", "images");
        Directory.CreateDirectory(uploadFolder);

        // Sanitize file name
        var baseName = Path.GetFileNameWithoutExtension(file.FileName);
        baseName = SanitizeFileNameRegex().Replace(baseName, "-");
        if (string.IsNullOrWhiteSpace(baseName))
            baseName = "img";

        var safeName = $"{baseName}-{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(uploadFolder, safeName);

        Console.WriteLine($"Saving file to: {fullPath}");

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        // Build URL (always forward slashes, works on Linux + Windows)
        var relativeUrl = $"/uploads/images/{safeName}".Replace("\\", "/"); 
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