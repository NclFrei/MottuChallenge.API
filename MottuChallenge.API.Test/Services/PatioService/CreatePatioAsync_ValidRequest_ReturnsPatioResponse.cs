using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.PatioTests
{
 public class CreatePatioAsync_ValidRequest_ReturnsPatioResponse
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IPatioRepository>();
 var mockValidator = new Mock<IValidator<PatioRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<Patio>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is PatioRequest pr)
 return new Patio { Nome = pr.Nome, UserId = pr.UserId ??0, Endereco = pr.Endereco != null ? new Endereco { Rua = pr.Endereco.Rua, Numero = pr.Endereco.Numero ??0, Bairro = pr.Endereco.Bairro } : null };
 return src as Patio ?? new Patio();
 });

 mockMapper.Setup(m => m.Map<Endereco>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is EnderecoRequest er)
 return new Endereco { Rua = er.Rua, Numero = er.Numero ??0, Bairro = er.Bairro, Cidade = er.Cidade, Estado = er.Estado, Cep = er.Cep, Complemento = er.Complemento };
 return src as Endereco ?? new Endereco();
 });

 mockMapper.Setup(m => m.Map<PatioResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Patio p)
 return new PatioResponse { Id = p.Id, Nome = p.Nome, UserId = p.UserId, Endereco = p.Endereco != null ? new EnderecoResponse { Id = p.Endereco.Id, Rua = p.Endereco.Rua, Numero = p.Endereco.Numero, Bairro = p.Endereco.Bairro } : null };
 return null!;
 });

 var request = new PatioRequest { Nome = "P1", UserId =2, Endereco = new EnderecoRequest { Rua = "R", Numero =10 } };
 var created = new Patio { Id =5, Nome = "P1", UserId =2, Endereco = new Endereco { Id =3, Rua = "R", Numero =10 } };

 mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
 mockRepo.Setup(r => r.CreateAsync(It.IsAny<Patio>(), It.IsAny<Endereco>())).ReturnsAsync(created);

 var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.CreatePatioAsync(request);

 Assert.NotNull(result);
 Assert.Equal(created.Id, result.Id);
 Assert.Equal(created.Nome, result.Nome);
 Assert.Equal(created.UserId, result.UserId);
 }
 }
}