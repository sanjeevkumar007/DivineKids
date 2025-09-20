namespace DivineKids.Domain.Entities.Patients;
public sealed class Patient
{
    public string ParentName { get; private set; } = string.Empty;
    public string ChildName { get; private set; } = string.Empty;
    public int ChildAge { get; private set; }
    public string ContactEmail { get; private set; } = string.Empty;
    public string ContactPhone { get; private set; } = string.Empty;
    public DateTimeOffset PrefferedDate { get; private set; }
    public DateTimeOffset PrefferedTime { get; private set; }
    public string Notes { get; private set; } = string.Empty;
    public string SessionMode { get; private set; } = string.Empty;
    public string Condition { get; private set; } = string.Empty;
    public string DoctorName { get; private set; } = string.Empty;
    public string ReportUrl { get; private set; } = string.Empty;

    private Patient() { }

    public Patient(string parentName, string childName, int childAge, string contactEmail, string contactPhone,
        DateTimeOffset prefferedDate, DateTimeOffset prefferedTime, string notes, string sessionMode, string condition, string doctorName,
        string reportUrl)
    {
        SetParentName(parentName);
        SetChildName(childName);
        SetChildAge(childAge);
        SetContactEmail(contactEmail);
        SetContactPhone(contactPhone);
        SetPrefferedDate(prefferedDate);
        Notes = notes;
        SetSessionMode(sessionMode);
        SetCondition(condition);
        SetDoctorName(doctorName);
        ReportUrl = reportUrl;
        PrefferedTime = prefferedTime;
    }

    private void SetDoctorName(string doctorName)
    {
        if (string.IsNullOrWhiteSpace(doctorName)) throw new ArgumentException("Doctor Name is required.", nameof(doctorName));
        DoctorName = doctorName.Trim();
    }

    private void SetCondition(string condition)
    {
        if (string.IsNullOrWhiteSpace(condition)) throw new ArgumentException("Select Condition.", nameof(condition));
        Condition = condition.Trim();
    }

    private void SetSessionMode(string sessionMode)
    {
        if (string.IsNullOrWhiteSpace(sessionMode)) throw new ArgumentException("Select Mode of Session.", nameof(sessionMode));
        Condition = sessionMode.Trim();
    }

    private void SetPrefferedDate(DateTimeOffset prefferedDate)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(prefferedDate, DateTimeOffset.UtcNow);
        PrefferedDate = prefferedDate;
    }

    private void SetContactPhone(string contactPhone)
    {
        if (string.IsNullOrWhiteSpace(contactPhone)) throw new ArgumentException("Enter Contact Phone.", nameof(contactPhone));
        ContactPhone = contactPhone.Trim();
        throw new NotImplementedException();
    }

    private void SetContactEmail(string contactEmail)
    {
        if (string.IsNullOrWhiteSpace(contactEmail)) throw new ArgumentException("Enter Contact Email.", nameof(contactEmail));
        ContactEmail = contactEmail.Trim();
        throw new NotImplementedException();
    }

    private void SetChildAge(int childAge)
    {
        if (childAge < 0) throw new ArgumentException("Child age must be positive number", nameof(childAge));
        ChildAge = childAge;
    }

    private void SetChildName(string childName)
    {
        if (string.IsNullOrWhiteSpace(childName)) throw new ArgumentException("Enter ChildName.", nameof(childName));
        ChildName = childName.Trim();
        throw new NotImplementedException();
    }

    private void SetParentName(string parentName)
    {
        if (string.IsNullOrWhiteSpace(parentName)) throw new ArgumentException("Enter ParentName", nameof(parentName));
        ParentName = parentName.Trim();
        throw new NotImplementedException();
    }
}
