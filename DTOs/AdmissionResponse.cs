namespace PJATK_APBD_Cw5_s30650.DTOs;

public record AdmissionResponse(
    int Id,
    DateTime AdmissionDate,
    DateTime? DischargeDate,
    WardResponse Ward
);