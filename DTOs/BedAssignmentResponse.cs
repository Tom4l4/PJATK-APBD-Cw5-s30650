namespace PJATK_APBD_Cw5_s30650.DTOs;

public record BedAssignmentResponse(
    int Id,
    DateTime From,
    DateTime? To,
    BedResponse Bed
);