using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Application.Features.EnvironmentalData;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;
using ValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;
using EnvironmentalDataEntity = BomberosAPI.Domain.Entities.EnvironmentalData;

namespace BomberosAPI.Application.UnitTests.EnvironmentalData.Services;

public class EnvironmentalDataServiceTests
{
    private readonly Mock<IEnvironmentalDataRepository> _mockRepo;
    private readonly Mock<ITrainingSessionRepository> _mockSessionRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IValidator<CreateEnvironmentalDataRequest>> _mockValidator;
    private readonly EnvironmentalDataService _sut;

    public EnvironmentalDataServiceTests()
    {
        _mockRepo = new Mock<IEnvironmentalDataRepository>();
        _mockSessionRepo = new Mock<ITrainingSessionRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockValidator = new Mock<IValidator<CreateEnvironmentalDataRequest>>();

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateEnvironmentalDataRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _sut = new EnvironmentalDataService(
            _mockRepo.Object, _mockSessionRepo.Object, _mockUserRepo.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var items = new List<EnvironmentalDataEntity>
        {
            new EnvironmentalDataEntity { EnvironmentalDataId = Guid.NewGuid(), TemperatureC = 25.5m }
        };
        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(items);

        var result = await _sut.GetAllAsync();

        result.Should().NotBeEmpty();
        result[0].TemperatureC.Should().Be(25.5m);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var item = new EnvironmentalDataEntity { EnvironmentalDataId = id, HumidityPct = 60 };
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(item);

        var result = await _sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result.HumidityPct.Should().Be(60);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((EnvironmentalDataEntity)null!);

        var act = async () => await _sut.GetByIdAsync(id);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetBySessionAsync_ReturnsList()
    {
        var id = Guid.NewGuid();
        var items = new List<EnvironmentalDataEntity> { new EnvironmentalDataEntity { EnvironmentalDataId = Guid.NewGuid(), TrainingSessionId = id } };
        _mockRepo.Setup(r => r.GetBySessionAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(items);

        var result = await _sut.GetBySessionAsync(id);

        result.Should().NotBeEmpty();
        result[0].TrainingSessionId.Should().Be(id);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsDto()
    {
        var sessionId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var request = new CreateEnvironmentalDataRequest(sessionId, userId, 25.5m, 60m, 10, 27m);

        _mockSessionRepo.Setup(r => r.GetByIdAsync(sessionId, It.IsAny<CancellationToken>())).ReturnsAsync(new TrainingSession());
        _mockUserRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new User());

        var result = await _sut.CreateAsync(request);

        result.Should().NotBeNull();
        result.TemperatureC.Should().Be(25.5m);
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<EnvironmentalDataEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_InvalidRequest_ThrowsValidationException()
    {
        var request = new CreateEnvironmentalDataRequest(Guid.Empty, Guid.Empty, 0, 0, 0, 0);
        var validationResult = new ValidationResult(new[] { new ValidationFailure("TemperatureC", "Invalid") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        var act = async () => await _sut.CreateAsync(request);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateAsync_SessionNotFound_ThrowsNotFoundException()
    {
        var request = new CreateEnvironmentalDataRequest(Guid.NewGuid(), Guid.NewGuid(), 25.5m, 60m, 10, 27m);
        _mockSessionRepo.Setup(r => r.GetByIdAsync(request.TrainingSessionId, It.IsAny<CancellationToken>())).ReturnsAsync((TrainingSession)null!);

        var act = async () => await _sut.CreateAsync(request);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_UserNotFound_ThrowsNotFoundException()
    {
        var request = new CreateEnvironmentalDataRequest(Guid.NewGuid(), Guid.NewGuid(), 25.5m, 60m, 10, 27m);
        _mockSessionRepo.Setup(r => r.GetByIdAsync(request.TrainingSessionId, It.IsAny<CancellationToken>())).ReturnsAsync(new TrainingSession());
        _mockUserRepo.Setup(r => r.GetByIdAsync(request.RegisteredByUserId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null!);

        var act = async () => await _sut.CreateAsync(request);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
