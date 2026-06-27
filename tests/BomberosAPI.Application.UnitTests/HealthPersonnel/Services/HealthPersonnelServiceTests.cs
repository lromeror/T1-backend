using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Application.Features.HealthPersonnel;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;
using ValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;
using HealthPersonnelEntity = BomberosAPI.Domain.Entities.HealthPersonnel;

namespace BomberosAPI.Application.UnitTests.HealthPersonnel.Services;

public class HealthPersonnelServiceTests
{
    private readonly Mock<IHealthPersonnelRepository> _mockRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IValidator<CreateHealthPersonnelRequest>> _mockValidator;
    private readonly HealthPersonnelService _sut;

    public HealthPersonnelServiceTests()
    {
        _mockRepo = new Mock<IHealthPersonnelRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockValidator = new Mock<IValidator<CreateHealthPersonnelRequest>>();

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateHealthPersonnelRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _sut = new HealthPersonnelService(_mockRepo.Object, _mockUserRepo.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var items = new List<HealthPersonnelEntity>
        {
            new HealthPersonnelEntity { HealthPersonnelId = Guid.NewGuid(), Profession = "Doctor" }
        };
        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(items);

        var result = await _sut.GetAllAsync();

        result.Should().NotBeEmpty();
        result[0].Profession.Should().Be("Doctor");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var hp = new HealthPersonnelEntity { HealthPersonnelId = id, Profession = "Nurse" };
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(hp);

        var result = await _sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result.Profession.Should().Be("Nurse");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((HealthPersonnelEntity)null!);

        var act = async () => await _sut.GetByIdAsync(id);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsDto()
    {
        var userId = Guid.NewGuid();
        var request = new CreateHealthPersonnelRequest(userId, "Doc", "Spec", "123", true);

        _mockUserRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new User());
        _mockRepo.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((HealthPersonnelEntity)null!);

        var result = await _sut.CreateAsync(request);

        result.Should().NotBeNull();
        result.Profession.Should().Be("Doc");
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<HealthPersonnelEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_InvalidRequest_ThrowsValidationException()
    {
        var request = new CreateHealthPersonnelRequest(Guid.NewGuid(), "", "", "", true);
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Profession", "Required") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        var act = async () => await _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateAsync_UserNotFound_ThrowsNotFoundException()
    {
        var userId = Guid.NewGuid();
        var request = new CreateHealthPersonnelRequest(userId, "Doc", "Spec", "123", true);
        _mockUserRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null!);

        var act = async () => await _sut.CreateAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ProfileAlreadyExists_ThrowsConflictException()
    {
        var userId = Guid.NewGuid();
        var request = new CreateHealthPersonnelRequest(userId, "Doc", "Spec", "123", true);
        
        _mockUserRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new User());
        _mockRepo.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new HealthPersonnelEntity());

        var act = async () => await _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ConflictException>();
    }
}
