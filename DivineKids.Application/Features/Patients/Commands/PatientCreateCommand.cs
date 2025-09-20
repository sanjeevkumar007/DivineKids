using System.ComponentModel.DataAnnotations;
using DivineKids.Domain.Entities.Patients;
using Microsoft.AspNetCore.Http;

namespace DivineKids.Application.Features.Patients.Commands;

public sealed class PatientCreateCommand
{
    [Required, StringLength(100)]
    public string ParentName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string ChildName { get; set; } = string.Empty;

    [Range(0, 150)]
    public int ChildAge { get; set; }

    [Required, EmailAddress, StringLength(200)]
    public string ContactEmail { get; set; } = string.Empty;

    [Required, Phone, StringLength(30)]
    public string ContactPhone { get; set; } = string.Empty;

    [Required]
    public DateTimeOffset PreferredDate { get; set; }

    //[Required]
    //public DateTimeOffset PreferredTime { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    [Required, StringLength(50)]
    public string SessionMode { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Condition { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string DoctorName { get; set; } = string.Empty;

    public IFormFile ReportFile { get; set; }

    public void Validate()
    {
        if (PreferredDate <= DateTimeOffset.UtcNow)
            throw new ValidationException("PreferredDate must be in the future.");
    }

    public Patient ToEntity()
    {
        // NOTE: The Patient entity currently throws NotImplementedException in several setters
        // (SetParentName, SetChildName, SetContactEmail, SetContactPhone) and has a typo in SessionMode setter
        // (assigns Condition). Those must be fixed before this will work at runtime.
        Validate();

        return new Patient(
            parentName: ParentName,
            childName: ChildName,
            childAge: ChildAge,
            contactEmail: ContactEmail,
            contactPhone: ContactPhone,
            prefferedDate: PreferredDate,
            prefferedTime: DateTimeOffset.Now,
            notes: Notes ?? string.Empty,
            sessionMode: SessionMode,
            condition: Condition,
            doctorName: DoctorName,
            reportUrl: ReportFile.FileName
        );
    }
}