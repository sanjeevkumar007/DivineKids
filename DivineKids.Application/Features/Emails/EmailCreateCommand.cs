using DivineKids.Application.Features.Dtos.Email;

namespace DivineKids.Application.Features.Emails;
public sealed class EmailCreateCommand
{
    public string ToEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public string TextBody { get; set; } = string.Empty;
    public List<EmailAttachmentDto>? Attachments { get; init; } // Optional
}
