using DivineKids.Application.Features.Patients.Commands;
using System.Text;

namespace DivineKids.Application.Features.Patients;

public sealed class PatientsService : IPatientsService
{
    public Task<string> SetPatientsDetails(PatientCreateCommand command)
    {
        // Plain-text email body containing all patient submission details.
        var builder = new StringBuilder();

        builder.AppendLine("New Patient Registration Request");
        builder.AppendLine("================================");
        builder.AppendLine($"Submitted At      : {DateTimeOffset.UtcNow:yyyy-MM-dd HH:mm} UTC");
        builder.AppendLine();
        builder.AppendLine("Parent / Child");
        builder.AppendLine("----------------");
        builder.AppendLine($"Parent Name       : {command.ParentName}");
        builder.AppendLine($"Child Name        : {command.ChildName}");
        builder.AppendLine($"Child Age         : {command.ChildAge}");
        builder.AppendLine();
        builder.AppendLine("Contact");
        builder.AppendLine("----------------");
        builder.AppendLine($"Email             : {command.ContactEmail}");
        builder.AppendLine($"Phone             : {command.ContactPhone}");
        builder.AppendLine();
        builder.AppendLine("Session Details");
        builder.AppendLine("----------------");
        builder.AppendLine($"Preferred Date    : {command.PreferredDate:yyyy-MM-dd} (UTC)");
        // If PreferredTime is later enabled, add it here similarly.
        builder.AppendLine($"Session Mode      : {command.SessionMode}");
        builder.AppendLine($"Condition         : {command.Condition}");
        builder.AppendLine($"Doctor Name       : {command.DoctorName}");
        builder.AppendLine();
        builder.AppendLine("Attachments");
        builder.AppendLine("----------------");
        builder.AppendLine($"Report File       : {(command.ReportFile != null ? command.ReportFile.FileName : "None")}");
        builder.AppendLine();
        builder.AppendLine("Notes");
        builder.AppendLine("----------------");
        builder.AppendLine(string.IsNullOrWhiteSpace(command.Notes) ? "None provided." : command.Notes!.Trim());
        builder.AppendLine();
        builder.AppendLine("End of submission.");

        return Task.FromResult(builder.ToString());
    }
}