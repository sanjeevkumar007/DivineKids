namespace DivineKids.Application.Features.Dtos.Email;

public sealed record EmailDataDto(string ToEmail, string Subject, string HtmlBody, string? TextBody, IReadOnlyCollection<string> AttachmentFileNames);