using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.MotoTests
{
 public class CreateAsync_ValidRequest_ReturnsMotoResponse
 {
 private Mock<IMapper> CreateMapperMock()
 {
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<Moto>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is MotoRequest mr)
 return new Moto { Placa = mr.Placa, Modelo = mr.Modelo, AreaId = mr.AreaId ??0 };
 return src as Moto ?? new Moto();
 });

 mockMapper.Setup(m => m.Map<MotoResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Moto m)
 return new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo, AreaId = m.AreaId };
 return null!;
 });

 return mockMapper;
 }

 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IMotoRepository>();
 var mockValidator = new Mock<IValidator<MotoRequest>>();
 var mockMapper = CreateMapperMock();

 var request = new MotoRequest { Placa = "ABC1234", Modelo = "X", AreaId =1 };
 var moto = new Moto { Id =1, Placa = "ABC1234", Modelo = "X", AreaId =1 };

 mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
 mockRepo.Setup(r => r.CreateAsync(It.IsAny<Moto>())).ReturnsAsync(moto);

 var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.CreateAsync(request);

 Assert.NotNull(result);
 Assert.Equal(moto.Id, result.Id);
 Assert.Equal(moto.Placa, result.Placa);
 }
 }
}
