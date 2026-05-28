namespace PJATK_APBD_Cw5_s30650.DTOs;

public record BedResponse(
    int Id,
    BedTypeResponse BedType,
    RoomResponse Room
);