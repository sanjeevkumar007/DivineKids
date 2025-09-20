using DivineKids.Application.Common.Constants;
using DivineKids.Application.Common.Extentions;
using DivineKids.Application.Contracts;
using DivineKids.Application.Features.Emails;
using DivineKids.Application.Features.Patients;
using DivineKids.Application.Features.Patients.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DivineKids.Api.Controllers.Emails;
[Route("api/[controller]")]
[ApiController]
public class EmailController(IPatientsService patientsService, IEmailService emailService, ILogger<EmailController> logger) : ControllerBase
{
    private readonly IPatientsService _patientsService = patientsService ?? throw new ArgumentNullException(nameof(patientsService));
    private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    private readonly ILogger<EmailController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private const long MaxFileBytes = 5 * 1024 * 1024; // 5 MB

    [HttpPost("SendEmail")]
    [RequestSizeLimit(MaxFileBytes)]
    public async Task<bool> SendEmailAsync(PatientCreateCommand command, CancellationToken cancellationToken)
    {
        var result = await _patientsService.SetPatientsDetails(command);

        EmailFormCommand emailFormCommand = new()
        {
            ToEmail = EmailConstants.EmailTo,
            Attachments =
            [
                new Application.Features.Dtos.Email.EmailAttachmentDto
                {
                    ContentType = "application/json",
                    FileName = command.ReportFile.FileName,
                    Base64Data = ConvertToBase64(command.ReportFile, cancellationToken)
                }
            ],
            Subject = "Patient Details",
            TextBody = result,
            HtmlBody = result.ToHtml()

        };

        await _emailService.SendAsync(emailFormCommand, cancellationToken);

        if (result != null)
        {
            _logger.LogInformation("Email sent to {ToEmail}", command.ContactEmail);
            return true;
        }
        else
        {
            _logger.LogError("Failed to send email to {ToEmail}", command.ContactEmail);
            return false;
        }
    }

    private static string ConvertToBase64(IFormFile reportFile, CancellationToken cancellationToken)
    {
        // Read the file stream and convert to Base64 string
        using var ms = new MemoryStream();
        reportFile.CopyToAsync(ms, cancellationToken).GetAwaiter().GetResult();
        return Convert.ToBase64String(ms.ToArray());
    }
}
