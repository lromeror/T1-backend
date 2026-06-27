using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Application.Features.Participants;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;
using ValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.UnitTests.Participants.Services;

public class SessionParticipantServiceTests
{
    private readonly Mock<ISessionParticipantRepository> _mockRepo;
    private readonly Mock<ITrainingSessionRepository> _mockSessionRepo;
    private readonly Mock<ITraineeFirefighterRepository> _mockTraineeRepo;
    private readonly Mock<IValidator<CreateSessionParticipantRequest>> _mockValidator;
    private readonly SessionParticipantService _sut;

    public SessionParticipantServiceTests()
    {
        _mockRepo = new Mock<ISessionParticipantRepository>();
        _mockSessionRepo = new Mock<ITrainingSessionRepository>();
        _mockTraineeRepo = new Mock<ITraineeFirefighterRepository>();
        _mockValidator = new Mock<IValidator<CreateSessionParticipantRequest>>();

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateSessionParticipantRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _sut = new SessionParticipantService(
            _mockRepo.Object, _mockSessionRepo.Object, _mockTraineeRepo.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var items = new List<SessionParticipant>
        {
            new SessionParticipant { SessionParticipantId = Guid.NewGuid(), ParticipationStatus = "Invited" }
        };
        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(items);

        var result = await _sut.GetAllAsync();

        result.Should().NotBeEmpty();
        result[0].ParticipationStatus.Should().Be("Invited");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var item = new SessionParticipant { SessionParticipantId = id, ParticipationStatus = "Confirmed" };
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(item);

        var result = await _sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result.ParticipationStatus.Should().Be("Confirmed");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((SessionParticipant)null!);

        var act = async () => await _sut.GetByIdAsync(id);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsDto()
    {
        var sessionId = Guid.NewGuid();
        var traineeId = Guid.NewGuid();
        var request = new CreateSessionParticipantRequest(sessionId, traineeId, null);

        _mockSessionRepo.Setup(r => r.GetByIdAsync(sessionId, It.IsAny<CancellationToken>())).ReturnsAsync(new TrainingSession());
        _mockTraineeRepo.Setup(r => r.GetByIdAsync(traineeId, It.IsAny<CancellationToken>())).ReturnsAsync(new TraineeFirefighter());

        var result = await _sut.CreateAsync(request);

        result.Should().NotBeNull();
        result.ParticipationStatus.Should().Be("Invited");
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<SessionParticipant>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_InvalidRequest_ThrowsValidationException()
    {
        var request = new CreateSessionParticipantRequest(Guid.Empty, Guid.Empty, null);
        var validationResult = new ValidationResult(new[] { new ValidationFailure("TrainingSessionId", "Required") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        var act = async () => await _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateAsync_SessionNotFound_ThrowsNotFoundException()
    {
        var sessionId = Guid.NewGuid();
        var request = new CreateSessionParticipantRequest(sessionId, Guid.NewGuid(), null);
        _mockSessionRepo.Setup(r => r.GetByIdAsync(sessionId, It.IsAny<CancellationToken>())).ReturnsAsync((TrainingSession)null!);

        var act = async () => await _sut.CreateAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_TraineeNotFound_ThrowsNotFoundException()
    {
        var sessionId = Guid.NewGuid();
        var traineeId = Guid.NewGuid();
        var request = new CreateSessionParticipantRequest(sessionId, traineeId, null);
        _mockSessionRepo.Setup(r => r.GetByIdAsync(sessionId, It.IsAny<CancellationToken>())).ReturnsAsync(new TrainingSession());
        _mockTraineeRepo.Setup(r => r.GetByIdAsync(traineeId, It.IsAny<CancellationToken>())).ReturnsAsync((TraineeFirefighter)null!);

        var act = async () => await _sut.CreateAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CheckInAsync_Existing_UpdatesStatus()
    {
        var id = Guid.NewGuid();
        var item = new SessionParticipant { SessionParticipantId = id, ParticipationStatus = "Invited" };
        
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(item);

        await _sut.CheckInAsync(id);

        item.ParticipationStatus.Should().Be("CheckedIn");
        item.AttendanceConfirmed.Should().BeTrue();
        item.CheckInAt.Should().NotBeNull();
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<SessionParticipant>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
