using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentValidation;
using AppValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.Features.Users;

public class UserService
{
    private readonly IUserRepository _repo;
    private readonly IValidator<CreateUserRequest> _createValidator;

    public UserService(IUserRepository repo, IValidator<CreateUserRequest> createValidator)
    {
        _repo = repo;
        _createValidator = createValidator;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await _repo.GetAllAsync(ct);
        return users.Select(ToDto).ToList();
    }

    public async Task<IReadOnlyList<UserDto>> GetByRoleAsync(string roleCode, CancellationToken ct = default)
    {
        var users = await _repo.GetByRoleAsync(roleCode, ct);
        return users.Select(ToDto).ToList();
    }

    public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("User", id);
        return ToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var validation = await _createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new AppValidationException(errors);
        }

        if (await _repo.ExistsByEmailAsync(request.Email, ct))
            throw new ConflictException("Email already in use.");

        var user = new User
        {
            UserId = Guid.NewGuid(),
            InstitutionId = request.InstitutionId,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            AccountStatus = "active",
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(user, ct);
        return ToDto(user);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct = default)
    {
        var user = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("User", id);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Phone = request.Phone;

        await _repo.UpdateAsync(user, ct);
        return ToDto(user);
    }

    public async Task SetStatusAsync(Guid id, string status, CancellationToken ct = default)
    {
        var user = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("User", id);

        user.AccountStatus = status;
        await _repo.UpdateAsync(user, ct);
    }

    private static UserDto ToDto(User u) => new(
        u.UserId, u.InstitutionId, u.Email,
        u.FirstName, u.LastName, u.Phone,
        u.AccountStatus, u.CreatedAt);
}
