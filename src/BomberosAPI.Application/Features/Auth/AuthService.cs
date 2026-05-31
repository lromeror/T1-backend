using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Application.Common.Interfaces;
using FluentValidation;
using AppValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.Features.Auth;

public class AuthService
{
    private readonly IAuthRepository _repo;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtService;
    private readonly IValidator<LoginRequest> _validator;

    public AuthService(IAuthRepository repo, IPasswordHasher passwordHasher,
        IJwtTokenService jwtService, IValidator<LoginRequest> validator)
    {
        _repo = repo;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _validator = validator;
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var validation = await _validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new AppValidationException(errors);
        }

        var user = await _repo.FindUserByEmailAsync(request.Email, ct);
        if (user is null)
            throw new UnauthorizedException("Invalid credentials.");

        if (user.AccountStatus != "active")
            throw new BusinessRuleException("Account is inactive. Contact your administrator.");

        var credential = await _repo.FindCredentialByUserIdAsync(user.UserId, ct);
        if (credential is null || !_passwordHasher.Verify(request.Password, credential.PasswordHash))
            throw new UnauthorizedException("Invalid credentials.");

        if (credential.LockedUntil.HasValue && credential.LockedUntil > DateTime.UtcNow)
            throw new BusinessRuleException("Account is temporarily locked. Try again later.");

        var roles = await _repo.GetActiveRoleCodesByUserIdAsync(user.UserId, ct);
        var (token, expiresAt) = _jwtService.GenerateToken(user, roles);

        return new LoginResult(token, expiresAt, user.UserId, user.Email,
            user.FirstName, user.LastName, roles);
    }
}
