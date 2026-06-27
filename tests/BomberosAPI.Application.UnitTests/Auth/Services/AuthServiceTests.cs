using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Application.Common.Interfaces;
using BomberosAPI.Application.Features.Auth;
using BomberosAPI.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace BomberosAPI.Application.UnitTests.Auth.Services;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _mockRepo;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtTokenService> _mockJwtService;
    private readonly Mock<IValidator<LoginRequest>> _mockValidator;
    private readonly AuthService _sut; // System Under Test

    public AuthServiceTests()
    {
        _mockRepo = new Mock<IAuthRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtService = new Mock<IJwtTokenService>();
        _mockValidator = new Mock<IValidator<LoginRequest>>();

        // Configuración por defecto: la validación del request pasa exitosamente
        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _sut = new AuthService(
            _mockRepo.Object,
            _mockPasswordHasher.Object,
            _mockJwtService.Object,
            _mockValidator.Object);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsLoginResultWithToken()
    {
        // Arrange
        var request = new LoginRequest("test@bomberos.org", "password123");
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = request.Email,
            AccountStatus = "active",
            FirstName = "John",
            LastName = "Doe"
        };
        var credential = new UserCredential
        {
            UserId = user.UserId,
            PasswordHash = "hashed_password123"
        };
        var roles = new List<string> { "trainee" };
        var expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.mocked_token";
        var expectedExpiration = DateTime.UtcNow.AddHours(1);

        _mockRepo.Setup(r => r.FindUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        _mockRepo.Setup(r => r.FindCredentialByUserIdAsync(user.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(credential);
            
        _mockRepo.Setup(r => r.GetActiveRoleCodesByUserIdAsync(user.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(roles);

        _mockPasswordHasher.Setup(p => p.Verify(request.Password, credential.PasswordHash))
            .Returns(true);

        _mockJwtService.Setup(j => j.GenerateToken(user, roles))
            .Returns((expectedToken, expectedExpiration));

        // Act
        var result = await _sut.LoginAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be(expectedToken);
        result.Email.Should().Be(user.Email);
        result.Roles.Should().BeEquivalentTo(roles);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ThrowsUnauthorizedException()
    {
        // Arrange
        var request = new LoginRequest("notfound@bomberos.org", "password123");

        _mockRepo.Setup(r => r.FindUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act
        var act = async () => await _sut.LoginAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage("Invalid credentials.");
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedException()
    {
        // Arrange
        var request = new LoginRequest("test@bomberos.org", "wrong_password");
        var user = new User { UserId = Guid.NewGuid(), Email = request.Email, AccountStatus = "active" };
        var credential = new UserCredential { UserId = user.UserId, PasswordHash = "hashed_password123" };

        _mockRepo.Setup(r => r.FindUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockRepo.Setup(r => r.FindCredentialByUserIdAsync(user.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(credential);

        _mockPasswordHasher.Setup(p => p.Verify(request.Password, credential.PasswordHash))
            .Returns(false); // Simulated wrong password

        // Act
        var act = async () => await _sut.LoginAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage("Invalid credentials.");
    }

    [Fact]
    public async Task LoginAsync_AccountInactive_ThrowsBusinessRuleException()
    {
        // Arrange
        var request = new LoginRequest("test@bomberos.org", "password123");
        var user = new User { UserId = Guid.NewGuid(), Email = request.Email, AccountStatus = "inactive" }; // Status invalid

        _mockRepo.Setup(r => r.FindUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var act = async () => await _sut.LoginAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Account is inactive. Contact your administrator.");
    }
}
