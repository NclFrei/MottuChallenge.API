using AutoMapper;
using Moq;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MottuChallenge.API.Test.Services;

public class UserServiceTests
{
    private Mock<IMapper> CreateMapperMock()
    {
        var mockMapper = new Mock<IMapper>();

        mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is User u)
                return new UserResponse { Id = u.Id, Nome = u.Nome, Email = u.Email };
            return null!;
        });

        mockMapper.Setup(m => m.Map<List<UserResponse>>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is IEnumerable<User> list)
                return list.Select(u => new UserResponse { Id = u.Id, Nome = u.Nome, Email = u.Email }).ToList();
            return new List<UserResponse>();
        });

        mockMapper.Setup(m => m.Map<AtualizarUserRequest, User>(It.IsAny<AtualizarUserRequest>(), It.IsAny<User>())).Returns((AtualizarUserRequest src, User dest) =>
        {
            if (!string.IsNullOrWhiteSpace(src.Nome)) dest.Nome = src.Nome!;
            if (!string.IsNullOrWhiteSpace(src.Email)) dest.Email = src.Email!;
            return dest;
        });

        return mockMapper;
    }

    [Fact]
    public async Task GetUserByIdAsync_Existing_ReturnsUserResponse()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockMapper = CreateMapperMock();

        var user = new User { Id = 2, Nome = "U", Email = "u@x" };
        mockRepo.Setup(r => r.BuscarPorIdAsync(2)).ReturnsAsync(user);

        var service = new UserService(mockRepo.Object, mockMapper.Object);

        var result = await service.GetUserByIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_NonExisting_ReturnsNull()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.BuscarPorIdAsync(99)).ReturnsAsync((User?)null);

        var service = new UserService(mockRepo.Object, mockMapper.Object);

        var result = await service.GetUserByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Existing_ReturnsTrue()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockMapper = CreateMapperMock();

        var user = new User { Id = 3 };
        mockRepo.Setup(r => r.BuscarPorIdAsync(3)).ReturnsAsync(user);
        mockRepo.Setup(r => r.DeleteAsync(user)).ReturnsAsync(true);

        var service = new UserService(mockRepo.Object, mockMapper.Object);

        var result = await service.DeleteAsync(3);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExisting_ReturnsFalse()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.BuscarPorIdAsync(33)).ReturnsAsync((User?)null);

        var service = new UserService(mockRepo.Object, mockMapper.Object);

        var result = await service.DeleteAsync(33);

        Assert.False(result);
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsPagedResponse()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockMapper = CreateMapperMock();

        var users = new List<User>
        {
            new User { Id = 1, Nome = "A" },
            new User { Id = 2, Nome = "B" },
            new User { Id = 3, Nome = "C" }
        };

        var queryable = users.AsQueryable();
        mockRepo.Setup(r => r.Query()).Returns(queryable);

        var service = new UserService(mockRepo.Object, mockMapper.Object);

        var response = await service.GetAllUsersAsync(page: 1, limit: 2);

        Assert.NotNull(response);
        Assert.Equal(2, response.Data.Count());
        Assert.Equal(3, response.Total);
        Assert.NotNull(response.Next);
    }

    [Fact]
    public async Task AtualizarPerfilAsync_Existing_UpdatesAndReturns()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockMapper = CreateMapperMock();

        var existing = new User { Id = 12, Nome = "Old", Email = "old@x", Password = "" };
        var request = new AtualizarUserRequest { Nome = "New", Email = "new@x", Password = "pass" };

        mockRepo.Setup(r => r.BuscarPorIdAsync(12)).ReturnsAsync(existing);
        mockRepo.Setup(r => r.AtualizarAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);

        var service = new UserService(mockRepo.Object, mockMapper.Object);

        var result = await service.AtualizarPerfilAsync(12, request);

        Assert.NotNull(result);
        Assert.Equal("New", result.Nome);
        Assert.Equal("new@x", result.Email);
    }

    [Fact]
    public async Task AtualizarPerfilAsync_NonExisting_ThrowsInvalidOperationException()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockMapper = CreateMapperMock();

        var request = new AtualizarUserRequest { Nome = "X" };
        mockRepo.Setup(r => r.BuscarPorIdAsync(99)).ReturnsAsync((User?)null);

        var service = new UserService(mockRepo.Object, mockMapper.Object);

        await Assert.ThrowsAsync<System.InvalidOperationException>(() => service.AtualizarPerfilAsync(99, request));
    }
}
