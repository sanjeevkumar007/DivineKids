using DivineKids.Application.Features.Patients.Commands;

namespace DivineKids.Application.Features.Patients;
public interface IPatientsService
{
    public Task<string> SetPatientsDetails(PatientCreateCommand command);
}
