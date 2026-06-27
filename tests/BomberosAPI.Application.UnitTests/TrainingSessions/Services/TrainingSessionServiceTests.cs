using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Application.Common.Interfaces;
using BomberosAPI.Application.Features.TrainingSessions;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;
using ValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.UnitTests.TrainingSessions.Services;

public class TrainingSessionServiceTests
{
    private readonly Mock<ITrainingSessionRepository> _mockRepo;
    private readonly Mock<ICurrentUserService> _mockCurrentUser;
    private readonly Mock<IValidator<CreateTrainingSessionRequest>> _mockValidator;
    private readonly TrainingSessionService _sut;

    public TrainingSessionServiceTests()
    {
        _mockRepo = new Mock<ITrainingSessionRepository>();
        _mockCurrentUser = new Mock<ICurrentUserService>();
        _mockValidator = new Mock<IValidator<CreateTrainingSessionRequest>>();

        _mockCurrentUser.SetupGet(c => c.UserId).Returns(Guid.NewGuid());

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTrainingSessionRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _sut = new TrainingSessionService(_mockRepo.Object, _mockCurrentUser.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var items = new List<TrainingSession>
        {
            new TrainingSession { TrainingSessionId = Guid.NewGuid(), Title = "Fire Drill" }
        };
        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(items);

        var result = await _sut.GetAllAsync();

        result.Should().NotBeEmpty();
        result[0].Title.Should().Be("Fire Drill");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var session = new TrainingSession { TrainingSessionId = id, Title = "Session 1" };
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(session);

        var result = await _sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result.Title.Should().Be("Session 1");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((TrainingSession)null!);

        var act = async () => await _sut.GetByIdAsync(id);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsDto()
    {
        var request = new CreateTrainingSessionRequest(Guid.NewGuid(), Guid.NewGuid(), "CODE", "Title", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddHours(2), 20);

        var result = await _sut.CreateAsync(request);

        result.Should().NotBeNull();
        result.Title.Should().Be("Title");
        result.Status.Should().Be("Scheduled");
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<TrainingSession>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_InvalidRequest_ThrowsValidationException()
    {
        var request = new CreateTrainingSessionRequest(Guid.Empty, Guid.Empty, "", "", "", DateTime.Now, DateTime.Now, 0);
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Title", "Required") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        var act = async () => await _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateAsync_ScheduledSession_UpdatesAndReturnsDto()
    {
        var id = Guid.NewGuid();
        var session = new TrainingSession { TrainingSessionId = id, Status = "Scheduled" };
        var request = new UpdateTrainingSessionRequest("New Title", "Desc", DateTime.Now, DateTime.Now, 10);
        
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(session);

        var result = await _sut.UpdateAsync(id, request);

        result.Title.Should().Be("New Title");
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<TrainingSession>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonScheduledSession_ThrowsBusinessRuleException()
    {
        var id = Guid.NewGuid();
        var session = new TrainingSession { TrainingSessionId = id, Status = "InProgress" };
        var request = new UpdateTrainingSessionRequest("New Title", "Desc", DateTime.Now, DateTime.Now, 10);
        
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(session);

        var act = async () => await _sut.UpdateAsync(id, request);

        await act.Should().ThrowAsync<BusinessRuleException>();
    }

    [Fact]
    public async Task StartAsync_ScheduledSession_ChangesStatusToInProgress()
    {
        var id = Guid.NewGuid();
        var session = new TrainingSession { TrainingSessionId = id, Status = "Scheduled" };
        
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(session);

        var result = await _sut.StartAsync(id);

        result.Status.Should().Be("InProgress");
        session.ActualStart.Should().NotBeNull();
    }

    [Fact]
    public async Task FinishAsync_InProgressSession_ChangesStatusToFinished()
    {
        var id = Guid.NewGuid();
        var session = new TrainingSession { TrainingSessionId = id, Status = "InProgress" };
        
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(session);

        var result = await _sut.FinishAsync(id);

        result.Status.Should().Be("Finished");
        session.ActualEnd.Should().NotBeNull();
    }
}
