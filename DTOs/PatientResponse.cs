namespace PJATK_APBD_Cw5_s30650.DTOs;

public record PatientResponse(
    string Pesel,
    string FirstName,
    string LastName,
    int Age,
    string Sex,
    IEnumerable<object> Admissions,
    IEnumerable<object> BedAssignments
    );