namespace DivineKids.Application.Features.Dtos.Email;
public sealed class EmailAttachmentDto
{
    public string FileName { get; init; } = string.Empty;
    public string Base64Data { get; init; } = string.Empty;
    public string? ContentType { get; init; }
    public long? LengthBytes { get; init; } // Optional: for validation/echo back
}