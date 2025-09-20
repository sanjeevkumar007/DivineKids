using DivineKids.Application.Common.Constants;
using DivineKids.Application.Contracts;
using DivineKids.Application.Features.Dtos.Email;
using DivineKids.Application.Features.Emails;
using DivineKids.Application.Features.Patients;
using DivineKids.Domain.Entities.Emails;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;

namespace DivineKids.Infrastructure.Email;

internal sealed class SmtpEmailService(IOptions<EmailSettings> options, ILogger<SmtpEmailService> logger) : IEmailService
{
    private readonly EmailSettings _settings = options.Value;
    private readonly ILogger<SmtpEmailService> _logger = logger;

    private const long MaxSingleAttachmentBytes = 5 * 1024 * 1024; // 5 MB
    private const long MaxTotalAttachmentsBytes = 12 * 1024 * 1024; // 12 MB
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        MediaTypeNames.Application.Pdf,
        MediaTypeNames.Image.Jpeg,
        MediaTypeNames.Image.Gif,
        MediaTypeNames.Image.Tiff,
        MediaTypeNames.Image.Png,
        MediaTypeNames.Text.Plain
    };

    // Existing Base64-based method (unchanged except positioned first)
    public async Task<EmailDataDto> SendAsync(EmailFormCommand command, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var attachmentNames = new List<string>();
        var entity = new EmailData(
            EmailConstants.EmailTo,
            command.Subject,
            command.HtmlBody,
            command.TextBody,
            attachmentFileNames: command.Attachments?.Select(a => a.FileName) ?? Enumerable.Empty<string>());

        using var message = BuildBaseMailMessage(entity);

        long totalBytes = 0;
        if (command.Attachments is { Count: > 0 })
        {
            foreach (var att in command.Attachments)
            {
                ValidateAttachment(att);
                byte[] bytes;
                try
                {
                    bytes = Convert.FromBase64String(att.Base64Data);
                }
                catch (FormatException)
                {
                    throw new InvalidOperationException($"Attachment {att.FileName} Base64 is invalid.");
                }

                var len = bytes.LongLength;
                EnforceSize(len, ref totalBytes, att.FileName);

                var stream = new MemoryStream(bytes);
                var contentType = att.ContentType is not null && AllowedContentTypes.Contains(att.ContentType)
                    ? att.ContentType
                    : MediaTypeNames.Application.Octet;

                message.Attachments.Add(new Attachment(stream, att.FileName, contentType));
                attachmentNames.Add(att.FileName);
            }
        }

        return await SendInternalAsync(message, entity, attachmentNames, ct);
    }

    // New IFormFile-based method
    public async Task<EmailDataDto> SendAsync(EmailFormCommand command, IReadOnlyCollection<IFormFile> attachments, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var attachmentNames = new List<string>();

        var entity = new EmailData(
            command.ToEmail,
            command.Subject,
            command.HtmlBody,
            command.TextBody ?? string.Empty,
            attachmentFileNames: attachments.Select(a => a.FileName));

        using var message = BuildBaseMailMessage(entity);

        long totalBytes = 0;
        if (attachments.Count > 0)
        {
            foreach (var file in attachments)
            {
                if (file.Length <= 0) continue;

                EnforceSize(file.Length, ref totalBytes, file.FileName);

                var contentType = !string.IsNullOrWhiteSpace(file.ContentType) && AllowedContentTypes.Contains(file.ContentType)
                    ? file.ContentType
                    : MediaTypeNames.Application.Octet;

                var ms = new MemoryStream();
                await file.CopyToAsync(ms, ct);
                ms.Position = 0;

                var attachment = new Attachment(ms, file.FileName, contentType);
                message.Attachments.Add(attachment);
                attachmentNames.Add(file.FileName);
            }
        }

        return await SendInternalAsync(message, entity, attachmentNames, ct);
    }

    private MailMessage BuildBaseMailMessage(EmailData entity)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress, _settings.FromName, Encoding.UTF8),
            Subject = entity.Subject,
            Body = entity.HtmlBody,
            IsBodyHtml = true,
            BodyEncoding = Encoding.UTF8,
            SubjectEncoding = Encoding.UTF8
        };
        message.To.Add(new MailAddress(entity.ToEmail));

        if (!string.IsNullOrWhiteSpace(entity.TextBody))
        {
            var altView = AlternateView.CreateAlternateViewFromString(
                entity.TextBody,
                Encoding.UTF8,
                MediaTypeNames.Text.Plain);
            message.AlternateViews.Add(altView);
        }

        return message;
    }

    private static void EnforceSize(long fileBytes, ref long totalBytes, string fileName)
    {
        if (fileBytes > MaxSingleAttachmentBytes)
            throw new InvalidOperationException($"Attachment {fileName} exceeds {MaxSingleAttachmentBytes / (1024 * 1024)} MB.");
        totalBytes += fileBytes;
        if (totalBytes > MaxTotalAttachmentsBytes)
            throw new InvalidOperationException($"Total attachments exceed {MaxTotalAttachmentsBytes / (1024 * 1024)} MB.");
    }

    private async Task<EmailDataDto> SendInternalAsync(MailMessage message, EmailData entity, List<string> attachmentNames, CancellationToken ct)
    {
        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.UseSsl,
            Credentials = new NetworkCredential(_settings.User, _settings.Password)
        };

        try
        {
            await client.SendMailAsync(message, ct);
            _logger.LogInformation("Email sent to {To} with {AttachmentCount} attachment(s)", entity.ToEmail, attachmentNames.Count);
            return MapToDto(entity, attachmentNames);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed sending email to {To}", entity.ToEmail);
            throw;
        }
    }

    private static void ValidateAttachment(EmailAttachmentDto att)
    {
        if (att is null) throw new ArgumentNullException(nameof(att));
        if (string.IsNullOrWhiteSpace(att.FileName))
            throw new InvalidOperationException("Attachment FileName required.");
        if (string.IsNullOrWhiteSpace(att.Base64Data))
            throw new InvalidOperationException($"Attachment {att.FileName} missing Base64Data.");
    }

    private static EmailDataDto MapToDto(EmailData emailData, IReadOnlyCollection<string> attachmentNames)
    {
        ArgumentNullException.ThrowIfNull(emailData);
        return new EmailDataDto(
            emailData.ToEmail,
            emailData.Subject,
            emailData.HtmlBody,
            emailData.TextBody,
            attachmentNames);
    }
}