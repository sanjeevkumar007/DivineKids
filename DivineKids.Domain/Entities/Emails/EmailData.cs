namespace DivineKids.Domain.Entities.Emails;

public class EmailData
{
    public string ToEmail { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string HtmlBody { get; private set; } = string.Empty;
    public string? TextBody { get; private set; }
    public IReadOnlyCollection<string> AttachmentFileNames => _attachmentFileNames;
    private readonly List<string> _attachmentFileNames = new();




    public EmailData()
    {

    }

    public EmailData(string toEmail, string subject, string htmlBody, string textBody, IEnumerable<string> attachmentFileNames)
    {
        SetToEmail(toEmail);
        Subject = subject;
        HtmlBody = htmlBody;
        TextBody = textBody;
        if (attachmentFileNames is not null)
            _attachmentFileNames.AddRange(attachmentFileNames);
    }

    private void SetToEmail(string toEmail)
    {
        if (string.IsNullOrWhiteSpace(toEmail)) throw new ArgumentException("Receiver email is required.", nameof(toEmail));
        ToEmail = toEmail.Trim();
    }
}
