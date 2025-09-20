using DivineKids.Application.Contracts;
using DivineKids.Application.Features.Dtos.Email;
using DivineKids.Application.Features.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DivineKids.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmailController(IEmailService emailService, ILogger<EmailController> logger) : ControllerBase
{
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<EmailController> _logger = logger;

    [HttpPost("send-json")]
    public async Task<ActionResult<EmailDataDto>> SendJsonAsync([FromBody] EmailCreateCommand command, CancellationToken ct)
    {
        //if (command is null) return BadRequest("Body required.");
        //var dto = await _emailService.SendAsync(command, ct);
        //return Ok(dto);
        return null;
    }

    [HttpPost("send-form")]
    [RequestSizeLimit(15 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<EmailDataDto>> SendFormAsync(
        [FromForm] EmailFormCommand form,
        [FromForm] List<IFormFile> attachments,
        CancellationToken ct)
    {
        if (form is null) return BadRequest("Form required.");
        if (string.IsNullOrWhiteSpace(form.ToEmail)) return BadRequest("ToEmail required.");
        if (string.IsNullOrWhiteSpace(form.Subject)) return BadRequest("Subject required.");
        if (string.IsNullOrWhiteSpace(form.HtmlBody)) return BadRequest("HtmlBody required.");

        try
        {
            var dto = await _emailService.SendAsync(form, attachments, ct);
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation failure sending (form) email to {To}", form.ToEmail);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error sending (form) email to {To}", form.ToEmail);
            return StatusCode(500, "Failed to send email.");
        }
    }
}