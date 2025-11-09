using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Test.Services.MotoTests
{
 public class GetByIdAsync_ExistingId_ReturnsMotoResponse
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IMotoRepository>();
 var mockValidator = new Mock<IValidator<MotoRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<MotoResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Moto m)
 return new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo, AreaId = m.AreaId };
 return null!;
 });

 var moto = new Moto { Id =5, Placa = "P5", Modelo = "M5", AreaId =2 };
 mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(moto);

 var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.GetByIdAsync(5);

 Assert.NotNull(result);
 Assert.Equal(moto.Id, result.Id);
 }
 }
}
