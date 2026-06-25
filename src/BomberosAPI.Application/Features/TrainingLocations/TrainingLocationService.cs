using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Domain.Repositories;

namespace BomberosAPI.Application.Features.TrainingLocations;

public class TrainingLocationService
{
    private readonly ITrainingLocationRepository _repo;

    public TrainingLocationService(ITrainingLocationRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<TrainingLocationDto>> GetAllAsync(CancellationToken ct = default)
    {
        var locations = await _repo.GetAllAsync(ct);
        return locations.Select(ToDto).ToList();
    }

    public async Task<TrainingLocationDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var loc = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("TrainingLocation", id);
        return ToDto(loc);
    }

    private static TrainingLocationDto ToDto(BomberosAPI.Domain.Entities.TrainingLocation l) => new(
        l.TrainingLocationId, l.InstitutionId, l.Name, l.LocationType, l.Address, l.MaxCapacity);
}
