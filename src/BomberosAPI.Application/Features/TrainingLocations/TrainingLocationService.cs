using BomberosAPI.Domain.Repositories;

namespace BomberosAPI.Application.Features.TrainingLocations;

public class TrainingLocationService
{
    private readonly ITrainingLocationRepository _repo;

    public TrainingLocationService(ITrainingLocationRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<TrainingLocationDto>> GetAllAsync(CancellationToken ct = default)
    {
        var locations = await _repo.GetAllAsync(ct);
        return locations.Select(l => new TrainingLocationDto(
            l.TrainingLocationId,
            l.InstitutionId,
            l.Name,
            l.LocationType,
            l.Address,
            l.MaxCapacity
        )).ToList();
    }
}
