namespace PJATK_APBD_Cw5_s30650.DTOs;

public record AssignBedRequest(
    DateTime From,
    DateTime? To,
    string BedType,
    string Ward
);