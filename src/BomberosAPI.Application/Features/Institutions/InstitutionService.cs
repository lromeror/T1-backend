using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentValidation;
using AppValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.Features.Institutions;

public class InstitutionService
{
    private readonly ITrainingInstitutionRepository _repo;
    private readonly IValidator<CreateInstitutionRequest> _createValidator;

    public InstitutionService(
        ITrainingInstitutionRepository repo,
        IValidator<CreateInstitutionRequest> createValidator)
    {
        _repo = repo;
        _createValidator = createValidator;
    }

    public async Task<IReadOnlyList<InstitutionDto>> GetAllAsync(CancellationToken ct = default)
    {
        var institutions = await _repo.GetAllAsync(ct);
        return institutions.Select(ToDto).ToList();
    }

    public async Task<InstitutionDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var institution = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Institution", id);
        return ToDto(institution);
    }

    public async Task<InstitutionDto> CreateAsync(CreateInstitutionRequest request, CancellationToken ct = default)
    {
        var validation = await _createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new AppValidationException(errors);
        }

        var institution = new TrainingInstitution
        {
            InstitutionId = Guid.NewGuid(),
            Name = request.Name,
            Acronym = request.Acronym,
            Country = request.Country,
            City = request.City,
            IsActive = true
        };

        await _repo.AddAsync(institution, ct);
        return ToDto(institution);
    }

    public async Task<InstitutionDto> UpdateAsync(Guid id, UpdateInstitutionRequest request, CancellationToken ct = default)
    {
        var institution = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Institution", id);

        institution.Name = request.Name;
        institution.Acronym = request.Acronym;
        institution.Country = request.Country;
        institution.City = request.City;

        await _repo.UpdateAsync(institution, ct);
        return ToDto(institution);
    }

    private static InstitutionDto ToDto(TrainingInstitution i) =>
        new(i.InstitutionId, i.Name, i.Acronym, i.Country, i.City, i.IsActive);
}
