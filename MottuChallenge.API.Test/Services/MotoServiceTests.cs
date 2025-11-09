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

public class MotoServiceTests
{
    private Mock<IMapper> CreateMapperMock()
    {
        var mockMapper = new Mock<IMapper>();

        mockMapper.Setup(m => m.Map<Moto>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is MotoRequest mr)
                return new Moto { Placa = mr.Placa, Modelo = mr.Modelo, AreaId = mr.AreaId ?? 0 };
            return src as Moto ?? new Moto();
        });

        mockMapper.Setup(m => m.Map<MotoResponse>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is Moto m)
                return new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo, AreaId = m.AreaId };
            return null!;
        });

        mockMapper.Setup(m => m.Map<List<MotoResponse>>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is IEnumerable<Moto> list)
                return list.Select(m => new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo, AreaId = m.AreaId }).ToList();
            return new List<MotoResponse>();
        });

        mockMapper.Setup(m => m.Map<AtualizarMotoRequest, Moto>(It.IsAny<AtualizarMotoRequest>(), It.IsAny<Moto>())).Returns((AtualizarMotoRequest src, Moto dest) =>
        {
            if (!string.IsNullOrWhiteSpace(src.Placa)) dest.Placa = src.Placa!;
            if (!string.IsNullOrWhiteSpace(src.Modelo)) dest.Modelo = src.Modelo!;
            if (src.AreaId.HasValue) dest.AreaId = src.AreaId.Value;
            return dest;
        });

        mockMapper.Setup(m => m.Map<MotoRequest, Moto>(It.IsAny<MotoRequest>(), It.IsAny<Moto>())).Returns((MotoRequest src, Moto dest) =>
        {
            dest.Placa = src.Placa!;
            dest.Modelo = src.Modelo!;
            dest.AreaId = src.AreaId ?? dest.AreaId;
            return dest;
        });

        return mockMapper;
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsMotoResponse()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new MotoRequest { Placa = "ABC1234", Modelo = "X", AreaId = 1 };
        var moto = new Moto { Id = 1, Placa = "ABC1234", Modelo = "X", AreaId = 1 };

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        mockRepo.Setup(r => r.CreateAsync(It.IsAny<Moto>())).ReturnsAsync(moto);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.CreateAsync(request);

        Assert.NotNull(result);
        Assert.Equal(moto.Id, result.Id);
        Assert.Equal(moto.Placa, result.Placa);
    }

    [Fact]
    public async Task CreateAsync_InvalidRequest_ThrowsValidationException()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new MotoRequest { Placa = "", Modelo = "X", AreaId = 1 };
        var failures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Placa", "Placa é obrigatória")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(failures);

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPagedResponse()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        var motos = new List<Moto>
        {
            new Moto { Id = 1, Placa = "A1", Modelo = "M1", AreaId = 1 },
            new Moto { Id = 2, Placa = "A2", Modelo = "M1", AreaId = 1 },
            new Moto { Id = 3, Placa = "A3", Modelo = "M2", AreaId = 2 }
        };

        var queryable = motos.AsQueryable();
        mockRepo.Setup(r => r.Query()).Returns(queryable);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var response = await service.GetAllAsync(null, null, page: 1, limit: 2);

        Assert.NotNull(response);
        Assert.Equal(2, response.Data.Count());
        Assert.Equal(3, response.Total);
        Assert.NotNull(response.Next);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsMotoResponse()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        var moto = new Moto { Id = 5, Placa = "P5", Modelo = "M5", AreaId = 2 };
        mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(moto);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.GetByIdAsync(5);

        Assert.NotNull(result);
        Assert.Equal(moto.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((Moto?)null);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.GetByIdAsync(10);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ExistingMoto_UpdatesAndReturns()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        var existing = new Moto { Id = 7, Placa = "OLD", Modelo = "OldM", AreaId = 3 };
        var request = new AtualizarMotoRequest { Placa = "NEW", Modelo = "NewM", AreaId = 4 };

        mockRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Moto>())).ReturnsAsync((Moto m) => m);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.UpdateAsync(7, request);

        Assert.NotNull(result);
        Assert.Equal("NEW", result.Placa);
        Assert.Equal("NewM", result.Modelo);
        Assert.Equal(4, result.AreaId);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingMoto_ReturnsNull()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new AtualizarMotoRequest { Placa = "X" };
        mockRepo.Setup(r => r.GetByIdAsync(20)).ReturnsAsync((Moto?)null);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.UpdateAsync(20, request);

        Assert.Null(result);
    }

    [Fact]
    public async Task ReplaceAsync_ValidRequest_ReplacesAndReturns()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new MotoRequest { Placa = "R1", Modelo = "RM", AreaId = 9 };
        var existing = new Moto { Id = 8, Placa = "Old", Modelo = "OM", AreaId = 1 };

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        mockRepo.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(existing);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Moto>())).ReturnsAsync((Moto m) => m);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.ReplaceAsync(8, request);

        Assert.NotNull(result);
        Assert.Equal(8, result.Id);
        Assert.Equal("R1", result.Placa);
        Assert.Equal("RM", result.Modelo);
        Assert.Equal(9, result.AreaId);
    }

    [Fact]
    public async Task ReplaceAsync_InvalidRequest_ThrowsValidationException()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new MotoRequest { Placa = "", Modelo = "RM", AreaId = 9 };
        var failures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Placa", "Placa é obrigatória")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(failures);

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.ReplaceAsync(8, request));
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueWhenDeleted()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.DeleteAsync(3)).ReturnsAsync(true);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.DeleteAsync(3);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseWhenNotFound()
    {
        var mockRepo = new Mock<IMotoRepository>();
        var mockValidator = new Mock<IValidator<MotoRequest>>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.DeleteAsync(33)).ReturnsAsync(false);

        var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.DeleteAsync(33);

        Assert.False(result);
    }
}
