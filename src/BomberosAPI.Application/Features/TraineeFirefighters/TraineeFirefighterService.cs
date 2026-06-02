using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentValidation;
using AppValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.Features.TraineeFirefighters;

public class TraineeFirefighterService
{
    private readonly ITraineeFirefighterRepository _repo;
    private readonly IValidator<CreateTraineeFirefighterRequest> _createValidator;

    public TraineeFirefighterService(
        ITraineeFirefighterRepository repo,
        IValidator<CreateTraineeFirefighterRequest> createValidator)
    {
        _repo = repo;
        _createValidator = createValidator;
    }

    public async Task<IReadOnlyList<TraineeFirefighterDto>> GetAllAsync(CancellationToken ct = default)
    {
        var trainees = await _repo.GetAllAsync(ct);
        return trainees.Select(ToDto).ToList();
    }

    public async Task<TraineeFirefighterDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var trainee = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("TraineeFirefighter", id);
        return ToDto(trainee);
    }

    public async Task<TraineeFirefighterDto> CreateAsync(CreateTraineeFirefighterRequest request, CancellationToken ct = default)
    {
        var validation = await _createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new AppValidationException(errors);
        }

        var trainee = new TraineeFirefighter
        {
            TraineeFirefighterId = Guid.NewGuid(),
            UserId = request.UserId,
            ApplicantCode = request.ApplicantCode,
            BirthDate = request.BirthDate,
            Sex = request.Sex,
            BloodType = request.BloodType,
            EmergencyContactName = request.EmergencyContactName,
            EmergencyContactPhone = request.EmergencyContactPhone,
            TrainingStatus = "Active"
        };

        await _repo.AddAsync(trainee, ct);
        return ToDto(trainee);
    }

    public async Task<TraineeFirefighterDto> UpdateAsync(Guid id, UpdateTraineeFirefighterRequest request, CancellationToken ct = default)
    {
        var trainee = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("TraineeFirefighter", id);

        trainee.BloodType = request.BloodType;
        trainee.EmergencyContactName = request.EmergencyContactName;
        trainee.EmergencyContactPhone = request.EmergencyContactPhone;

        await _repo.UpdateAsync(trainee, ct);
        return ToDto(trainee);
    }

    public async Task SetTrainingStatusAsync(Guid id, string status, CancellationToken ct = default)
    {
        var trainee = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("TraineeFirefighter", id);

        trainee.TrainingStatus = status;
        await _repo.UpdateAsync(trainee, ct);
    }

    private static TraineeFirefighterDto ToDto(TraineeFirefighter t) => new(
        t.TraineeFirefighterId, t.UserId, t.ApplicantCode,
        t.BirthDate, t.Sex, t.BloodType,
        t.EmergencyContactName, t.EmergencyContactPhone,
        t.TrainingStatus);
}