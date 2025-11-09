using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

public class AreaServiceTests
{
    private Mock<IMapper> CreateMapperMock()
    {
        var mockMapper = new Mock<IMapper>();

        mockMapper.Setup(m => m.Map<Area>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is AreaRequest ar)
                return new Area { Nome = ar.Nome, PatioId = ar.PatioId ?? 0 };
            return src as Area ?? new Area();
        });

        mockMapper.Setup(m => m.Map<AreaResponse>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is Area a)
                return new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId, Motos = a.Motos?.Select(m => new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo }).ToList() ?? new List<MotoResponse>() };
            return null!;
        });

        mockMapper.Setup(m => m.Map<List<AreaResponse>>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is IEnumerable<Area> list)
                return list.Select(a => new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId, Motos = a.Motos?.Select(m => new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo }).ToList() ?? new List<MotoResponse>() }).ToList();
            return new List<AreaResponse>();
        });

        mockMapper.Setup(m => m.Map<AtualizarAreaRequest, Area>(It.IsAny<AtualizarAreaRequest>(), It.IsAny<Area>())).Returns((AtualizarAreaRequest src, Area dest) =>
        {
            if (!string.IsNullOrWhiteSpace(src.Nome)) dest.Nome = src.Nome!;
            if (src.PatioId.HasValue) dest.PatioId = src.PatioId.Value;
            return dest;
        });

        mockMapper.Setup(m => m.Map<AreaRequest, Area>(It.IsAny<AreaRequest>(), It.IsAny<Area>())).Returns((AreaRequest src, Area dest) =>
        {
            dest.Nome = src.Nome!;
            dest.PatioId = src.PatioId ?? dest.PatioId;
            return dest;
        });

        return mockMapper;
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsAreaResponse()
    {
        // Arrange
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new AreaRequest { Nome = "A1", PatioId = 1 };
        var area = new Area { Id = 1, Nome = "A1", PatioId = 1 };

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        mockRepo.Setup(r => r.CreateAsync(It.IsAny<Area>())).ReturnsAsync(area);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(area.Id, result.Id);
        Assert.Equal(area.Nome, result.Nome);
    }

    [Fact]
    public async Task CreateAsync_InvalidRequest_ThrowsValidationException()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new AreaRequest { Nome = "", PatioId = 1 };
        var failures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Nome", "Nome é obrigatório")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(failures);

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPagedResponse()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        var areas = new List<Area>
        {
            new Area { Id = 1, Nome = "A1", PatioId = 1 },
            new Area { Id = 2, Nome = "A2", PatioId = 1 },
            new Area { Id = 3, Nome = "A3", PatioId = 2 }
        };

        var queryable = areas.AsQueryable();
        mockRepo.Setup(r => r.Query()).Returns(queryable);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var response = await service.GetAllAsync(null, null, page: 1, limit: 2);

        Assert.NotNull(response);
        Assert.Equal(2, response.Data.Count());
        Assert.Equal(3, response.Total);
        Assert.NotNull(response.Next);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsAreaResponse()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        var area = new Area { Id = 5, Nome = "Area5", PatioId = 2 };
        mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(area);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.GetByIdAsync(5);

        Assert.NotNull(result);
        Assert.Equal(area.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((Area?)null);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.GetByIdAsync(10);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ExistingArea_UpdatesAndReturns()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        var existing = new Area { Id = 7, Nome = "Old", PatioId = 3 };
        var request = new AtualizarAreaRequest { Nome = "New", PatioId = 4 };

        mockRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Area>())).ReturnsAsync((Area a) => a);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.UpdateAsync(7, request);

        Assert.NotNull(result);
        Assert.Equal("New", result.Nome);
        Assert.Equal(4, result.PatioId);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingArea_ReturnsNull()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new AtualizarAreaRequest { Nome = "X" };
        mockRepo.Setup(r => r.GetByIdAsync(20)).ReturnsAsync((Area?)null);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.UpdateAsync(20, request);

        Assert.Null(result);
    }

    [Fact]
    public async Task ReplaceAsync_ValidRequest_ReplacesAndReturns()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new AreaRequest { Nome = "R1", PatioId = 9 };
        var existing = new Area { Id = 8, Nome = "Old", PatioId = 1 };

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        mockRepo.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(existing);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Area>())).ReturnsAsync((Area a) => a);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.ReplaceAsync(8, request);

        Assert.NotNull(result);
        Assert.Equal(8, result.Id);
        Assert.Equal("R1", result.Nome);
        Assert.Equal(9, result.PatioId);
    }

    [Fact]
    public async Task ReplaceAsync_InvalidRequest_ThrowsValidationException()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new AreaRequest { Nome = "", PatioId = 9 };
        var failures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Nome", "Nome é obrigatório")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(failures);

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.ReplaceAsync(8, request));
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueWhenDeleted()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.DeleteAsync(3)).ReturnsAsync(true);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.DeleteAsync(3);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseWhenNotFound()
    {
        var mockRepo = new Mock<IAreaRepository>();
        var mockValidator = new Mock<IValidator<AreaRequest>>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.DeleteAsync(33)).ReturnsAsync(false);

        var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.DeleteAsync(33);

        Assert.False(result);
    }
}
