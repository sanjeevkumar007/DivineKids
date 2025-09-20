using DivineKids.Application.Features.Dtos.Email;
using DivineKids.Application.Features.Emails;
using DivineKids.Application.Features.Patients;
using Microsoft.AspNetCore.Http;

namespace DivineKids.Application.Contracts;

public interface IEmailService
{
    Task<EmailDataDto> SendAsync(EmailFormCommand command, CancellationToken ct = default);
    Task<EmailDataDto> SendAsync(EmailFormCommand command, IReadOnlyCollection<IFormFile> attachments, CancellationToken ct = default);
}