namespace DivineKids.Application.Features.Dtos.Patients;
public sealed record PatientDto(
    string ParentName,
    string ChildName,
    int ChildAge,
    string ContactEmail,
    string ContactPhone,
    DateTimeOffset PreferredDate,
    string Notes,
    string SessionMode,
    string Condition,
    string DoctorName,
    string ReportUrl);
