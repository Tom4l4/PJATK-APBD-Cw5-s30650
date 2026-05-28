namespace PJATK_APBD_Cw5_s30650.DTOs;

public record RoomResponse(
    string Id,
    bool HasTv,
    WardResponse Ward
);