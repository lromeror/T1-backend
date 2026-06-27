using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BomberosAPI.Application.Features.TrainingLocations;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace BomberosAPI.Application.UnitTests.TrainingLocations.Services;

public class TrainingLocationServiceTests
{
    private readonly Mock<ITrainingLocationRepository> _mockRepo;
    private readonly TrainingLocationService _sut;

    public TrainingLocationServiceTests()
    {
        _mockRepo = new Mock<ITrainingLocationRepository>();
        _sut = new TrainingLocationService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var locations = new List<TrainingLocation>
        {
            new TrainingLocation { TrainingLocationId = Guid.NewGuid(), Name = "Loc 1", LocationType = "Indoor" }
        };
        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(locations);

        var result = await _sut.GetAllAsync();

        result.Should().NotBeEmpty();
        result[0].Name.Should().Be("Loc 1");
        result[0].LocationType.Should().Be("Indoor");
    }
}
