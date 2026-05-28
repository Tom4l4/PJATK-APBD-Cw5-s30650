using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw5_s30650.Service;
using PJATK_APBD_Cw5_s30650.DTOs;
using PJATK_APBD_Cw5_s30650.Exceptions;

namespace PJATK_APBD_Cw5_s30650.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllAsync(search, cancellationToken));
    }
    
    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AssignBed(
        [FromRoute] string pesel,
        [FromBody] AssignBedRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await service.AssignBedAsync(pesel, request, cancellationToken);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}