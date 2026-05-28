using PJATK_APBD_Cw5_s30650.DTOs;

namespace PJATK_APBD_Cw5_s30650.Service;

public interface IPatientService
{
    Task<IEnumerable<PatientResponse>> GetAllAsync(string? search, CancellationToken cancellationToken);
    Task AssignBedAsync(string pesel, AssignBedRequest request, CancellationToken cancellationToken);
}