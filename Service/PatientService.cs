using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Cw5_s30650.DTOs;
using PJATK_APBD_Cw5_s30650.Models;
using PJATK_APBD_Cw5_s30650.Exceptions;

namespace PJATK_APBD_Cw5_s30650.Service;

public class PatientService(HospitalContext ctx) : IPatientService
{
    public async Task<IEnumerable<PatientResponse>> GetAllAsync(string? search, CancellationToken cancellationToken)
    {
        var query = ctx.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName, $"%{search}%") ||
                EF.Functions.Like(p.LastName, $"%{search}%"));
        }

        return await query.Select(p => new PatientResponse(
            p.Pesel,
            p.FirstName,
            p.LastName,
            p.Age,
            p.Sex ? "Male" : "Female",
            p.Admissions.Select(a => new AdmissionResponse(
                a.Id,
                a.AdmissionDate,
                a.DischargeDate,
                new WardResponse(
                    a.Ward.Id,
                    a.Ward.Name,
                    a.Ward.Description
                )
            )),
            p.BedAssignments.Select(ba => new BedAssignmentResponse(
                ba.Id,
                ba.From,
                ba.To,
                new BedResponse(
                    ba.Bed.Id,
                    new BedTypeResponse(
                        ba.Bed.BedType.Id,
                        ba.Bed.BedType.Name,
                        ba.Bed.BedType.Description
                    ),
                    new RoomResponse(
                        ba.Bed.Room.Id,
                        ba.Bed.Room.HasTv,
                        new WardResponse(
                            ba.Bed.Room.Ward.Id,
                            ba.Bed.Room.Ward.Name,
                            ba.Bed.Room.Ward.Description
                        )
                    )
                )
            ))
        )).ToListAsync(cancellationToken);
    }

    public async Task AssignBedAsync(string pesel, AssignBedRequest request, CancellationToken cancellationToken)
    {
        var patientExists = await ctx.Patients
            .AnyAsync(p => p.Pesel == pesel, cancellationToken);

        if (!patientExists)
        {
            throw new NotFoundException($"Patient with pesel {pesel} not found");
        }

        var wardExists = await ctx.Wards
            .AnyAsync(w => w.Name == request.Ward, cancellationToken);

        if (!wardExists)
        {
            throw new NotFoundException($"Ward {request.Ward} not found");
        }

        var bedTypeExists = await ctx.BedTypes
            .AnyAsync(bt => bt.Name == request.BedType, cancellationToken);

        if (!bedTypeExists)
        {
            throw new NotFoundException($"Bed type {request.BedType} not found");
        }

        var bed = await ctx.Beds
            .Where(b =>
                b.BedType.Name == request.BedType &&
                b.Room.Ward.Name == request.Ward &&
                !b.BedAssignments.Any(ba =>
                    ba.From < (request.To ?? new DateTime(9999, 12, 31)) &&
                    (ba.To == null || ba.To > request.From)
                ))
            .FirstOrDefaultAsync(cancellationToken);

        if (bed is null)
        {
            throw new NotFoundException("No available bed found");
        }

        var assignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = bed.Id,
            From = request.From,
            To = request.To
        };

        ctx.BedAssignments.Add(assignment);
        await ctx.SaveChangesAsync(cancellationToken);
    }
}