using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Application.Features.Institutions;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;
using ValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.UnitTests.Institutions.Services;

public class InstitutionServiceTests
{
    private readonly Mock<ITrainingInstitutionRepository> _mockRepo;
    private readonly Mock<IValidator<CreateInstitutionRequest>> _mockValidator;
    private readonly InstitutionService _sut;

    public InstitutionServiceTests()
    {
        _mockRepo = new Mock<ITrainingInstitutionRepository>();
        _mockValidator = new Mock<IValidator<CreateInstitutionRequest>>();

        // Valid by default
        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateInstitutionRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _sut = new InstitutionService(_mockRepo.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        // Arrange
        var institutions = new List<TrainingInstitution>
        {
            new TrainingInstitution { InstitutionId = Guid.NewGuid(), Name = "Inst1", Acronym = "I1" }
        };
        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(institutions);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().NotBeEmpty();
        result[0].Name.Should().Be("Inst1");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var institution = new TrainingInstitution { InstitutionId = id, Name = "Inst1" };
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(institution);

        var result = await _sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result.InstitutionId.Should().Be(id);
        result.Name.Should().Be("Inst1");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((TrainingInstitution)null!);

        var act = async () => await _sut.GetByIdAsync(id);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsDto()
    {
        var request = new CreateInstitutionRequest("Test Inst", "TI", "Country", "City");

        var result = await _sut.CreateAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be("Test Inst");
        result.IsActive.Should().BeTrue();
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<TrainingInstitution>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_InvalidRequest_ThrowsValidationException()
    {
        var request = new CreateInstitutionRequest("", "TI", "Country", "City");
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Name", "Name is required") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        var act = async () => await _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<TrainingInstitution>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_UpdatesAndReturnsDto()
    {
        var id = Guid.NewGuid();
        var institution = new TrainingInstitution { InstitutionId = id, Name = "Old Name" };
        var request = new UpdateInstitutionRequest("New Name", "NN", "Country", "City");

        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(institution);

        var result = await _sut.UpdateAsync(id, request);

        result.Should().NotBeNull();
        result.Name.Should().Be("New Name");
        _mockRepo.Verify(r => r.UpdateAsync(It.Is<TrainingInstitution>(i => i.Name == "New Name"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        var request = new UpdateInstitutionRequest("New Name", "NN", "Country", "City");
        _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((TrainingInstitution)null!);

        var act = async () => await _sut.UpdateAsync(id, request);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
