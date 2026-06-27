using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BomberosAPI.Application.Features.Roles;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace BomberosAPI.Application.UnitTests.Roles.Services;

public class RoleServiceTests
{
    private readonly Mock<IRoleRepository> _mockRepo;
    private readonly RoleService _sut;

    public RoleServiceTests()
    {
        _mockRepo = new Mock<IRoleRepository>();
        _sut = new RoleService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var roles = new List<Role> { new Role { RoleId = Guid.NewGuid(), Name = "Admin" } };
        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(roles);

        var result = await _sut.GetAllAsync();

        result.Should().NotBeEmpty();
        result[0].Name.Should().Be("Admin");
    }
}
