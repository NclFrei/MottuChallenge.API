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

public class PatioServiceTests
{
    private Mock<IMapper> CreateMapperMock()
    {
        var mockMapper = new Mock<IMapper>();

        mockMapper.Setup(m => m.Map<Patio>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is PatioRequest pr)
                return new Patio { Nome = pr.Nome, UserId = pr.UserId ?? 0, Endereco = pr.Endereco != null ? new Endereco { Rua = pr.Endereco.Rua, Numero = pr.Endereco.Numero ?? 0, Bairro = pr.Endereco.Bairro } : null };
            return src as Patio ?? new Patio();
        });

        mockMapper.Setup(m => m.Map<Endereco>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is EnderecoRequest er)
                return new Endereco { Rua = er.Rua, Numero = er.Numero ?? 0, Bairro = er.Bairro, Cidade = er.Cidade, Estado = er.Estado, Cep = er.Cep, Complemento = er.Complemento };
            return src as Endereco ?? new Endereco();
        });

        mockMapper.Setup(m => m.Map<PatioResponse>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is Patio p)
                return new PatioResponse { Id = p.Id, Nome = p.Nome, UserId = p.UserId, Endereco = p.Endereco != null ? new EnderecoResponse { Id = p.Endereco.Id, Rua = p.Endereco.Rua, Numero = p.Endereco.Numero, Bairro = p.Endereco.Bairro } : null, Areas = p.Areas?.Select(a => new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId }).ToList() ?? new List<AreaResponse>() };
            return null!;
        });

        mockMapper.Setup(m => m.Map<List<PatioResponse>>(It.IsAny<object>())).Returns((object src) =>
        {
            if (src is IEnumerable<Patio> list)
                return list.Select(p => new PatioResponse { Id = p.Id, Nome = p.Nome, UserId = p.UserId, Endereco = p.Endereco != null ? new EnderecoResponse { Id = p.Endereco.Id, Rua = p.Endereco.Rua, Numero = p.Endereco.Numero, Bairro = p.Endereco.Bairro } : null, Areas = p.Areas?.Select(a => new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId }).ToList() ?? new List<AreaResponse>() }).ToList();
            return new List<PatioResponse>();
        });

        mockMapper.Setup(m => m.Map<AtualizarPatioRequest, Patio>(It.IsAny<AtualizarPatioRequest>(), It.IsAny<Patio>())).Returns((AtualizarPatioRequest src, Patio dest) =>
        {
            if (!string.IsNullOrWhiteSpace(src.Nome)) dest.Nome = src.Nome!;
            if (src.UserId.HasValue) dest.UserId = src.UserId.Value;
            return dest;
        });

        mockMapper.Setup(m => m.Map<PatioRequest, Patio>(It.IsAny<PatioRequest>(), It.IsAny<Patio>())).Returns((PatioRequest src, Patio dest) =>
        {
            dest.Nome = src.Nome!;
            dest.UserId = src.UserId ?? dest.UserId;
            return dest;
        });

        mockMapper.Setup(m => m.Map<EnderecoRequest, Endereco>(It.IsAny<EnderecoRequest>(), It.IsAny<Endereco>())).Returns((EnderecoRequest src, Endereco dest) =>
        {
            dest.Rua = src.Rua;
            dest.Numero = src.Numero ?? dest.Numero;
            dest.Bairro = src.Bairro;
            dest.Cidade = src.Cidade;
            dest.Estado = src.Estado;
            dest.Cep = src.Cep;
            dest.Complemento = src.Complemento;
            return dest;
        });

        return mockMapper;
    }

    [Fact]
    public async Task CreatePatioAsync_ValidRequest_ReturnsPatioResponse()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new PatioRequest { Nome = "P1", UserId = 2, Endereco = new EnderecoRequest { Rua = "R", Numero = 10 } };
        var created = new Patio { Id = 5, Nome = "P1", UserId = 2, Endereco = new Endereco { Id = 3, Rua = "R", Numero = 10 } };

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        mockRepo.Setup(r => r.CreateAsync(It.IsAny<Patio>(), It.IsAny<Endereco>())).ReturnsAsync(created);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.CreatePatioAsync(request);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.Nome, result.Nome);
        Assert.Equal(created.UserId, result.UserId);
    }

    [Fact]
    public async Task CreatePatioAsync_InvalidRequest_ThrowsValidationException()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new PatioRequest { Nome = "" , Endereco = null };
        var failures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Nome", "Nome é obrigatório")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(failures);

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.CreatePatioAsync(request));
    }

    [Fact]
    public async Task GetAllPatiosAsync_ReturnsPagedResponse()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        var patios = new List<Patio>
        {
            new Patio { Id = 1, Nome = "P1" },
            new Patio { Id = 2, Nome = "P2" },
            new Patio { Id = 3, Nome = "P3" }
        };

        var queryable = patios.AsQueryable();
        mockRepo.Setup(r => r.Query()).Returns(queryable);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var response = await service.GetAllPatiosAsync(page: 1, limit: 2);

        Assert.NotNull(response);
        Assert.Equal(2, response.Data.Count());
        Assert.Equal(3, response.Total);
        Assert.NotNull(response.Next);
    }

    [Fact]
    public async Task GetPatioByUserIdAsync_Existing_ReturnsPatioResponse()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        var patio = new Patio { Id = 7, Nome = "X" };
        // Service currently calls GetByIdAsync with userid parameter
        mockRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(patio);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.GetPatioByUserIdAsync(7);

        Assert.NotNull(result);
        Assert.Equal(patio.Id, result.Id);
    }

    [Fact]
    public async Task GetPatioByUserIdAsync_NonExisting_ReturnsNull()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Patio?)null);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.GetPatioByUserIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetPatioByIdAsync_Existing_ReturnsPatioResponse()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        var patio = new Patio { Id = 11, Nome = "IdPatio" };
        mockRepo.Setup(r => r.GetByIdAsync(11)).ReturnsAsync(patio);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.GetPatioByIdAsync(11);

        Assert.NotNull(result);
        Assert.Equal(patio.Id, result.Id);
    }

    [Fact]
    public async Task UpdatePatioAsync_Existing_UpdatesAndReturns()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        var existing = new Patio { Id = 21, Nome = "Old", Endereco = null };
        var request = new AtualizarPatioRequest { Nome = "New", Endereco = new EnderecoRequest { Rua = "R" } };

        mockRepo.Setup(r => r.GetByIdAsync(21)).ReturnsAsync(existing);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Patio>())).ReturnsAsync((Patio p) => p);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.UpdatePatioAsync(21, request);

        Assert.NotNull(result);
        Assert.Equal("New", result.Nome);
        Assert.NotNull(result.Endereco);
    }

    [Fact]
    public async Task UpdatePatioAsync_NonExisting_ReturnsNull()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new AtualizarPatioRequest { Nome = "X" };
        mockRepo.Setup(r => r.GetByIdAsync(50)).ReturnsAsync((Patio?)null);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.UpdatePatioAsync(50, request);

        Assert.Null(result);
    }

    [Fact]
    public async Task ReplacePatioAsync_Valid_ReplacesAndReturns()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new PatioRequest { Nome = "R1", UserId = 9, Endereco = new EnderecoRequest { Rua = "Rua" } };
        var existing = new Patio { Id = 8, Nome = "Old" };

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        mockRepo.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(existing);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Patio>())).ReturnsAsync((Patio p) => p);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.ReplacePatioAsync(8, request);

        Assert.NotNull(result);
        Assert.Equal(8, result.Id);
        Assert.Equal("R1", result.Nome);
    }

    [Fact]
    public async Task ReplacePatioAsync_Invalid_ThrowsValidationException()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        var request = new PatioRequest { Nome = "" , Endereco = null };

        var failures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Nome", "Nome é obrigatório")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(failures);

        mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.ReplacePatioAsync(8, request));
    }

    [Fact]
    public async Task DeletePatioAsync_ReturnsTrueWhenDeleted()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.DeleteAsync(3)).ReturnsAsync(true);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.DeletePatioAsync(3);

        Assert.True(result);
    }

    [Fact]
    public async Task DeletePatioAsync_ReturnsFalseWhenNotFound()
    {
        var mockRepo = new Mock<IPatioRepository>();
        var mockValidator = new Mock<IValidator<PatioRequest>>();
        var mockMapper = CreateMapperMock();

        mockRepo.Setup(r => r.DeleteAsync(33)).ReturnsAsync(false);

        var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

        var result = await service.DeletePatioAsync(33);

        Assert.False(result);
    }
}
