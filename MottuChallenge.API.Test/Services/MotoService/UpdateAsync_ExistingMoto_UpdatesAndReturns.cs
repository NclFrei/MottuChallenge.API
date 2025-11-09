using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Test.Services.MotoTests
{
 public class UpdateAsync_ExistingMoto_UpdatesAndReturns
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IMotoRepository>();
 var mockValidator = new Mock<IValidator<MotoRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<AtualizarMotoRequest, Moto>(It.IsAny<AtualizarMotoRequest>(), It.IsAny<Moto>())).Returns((AtualizarMotoRequest src, Moto dest) =>
 {
 if (!string.IsNullOrWhiteSpace(src.Placa)) dest.Placa = src.Placa!;
 if (!string.IsNullOrWhiteSpace(src.Modelo)) dest.Modelo = src.Modelo!;
 if (src.AreaId.HasValue) dest.AreaId = src.AreaId.Value;
 return dest;
 });

 mockMapper.Setup(m => m.Map<MotoResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Moto m)
 return new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo, AreaId = m.AreaId };
 return null!;
 });

 var existing = new Moto { Id =7, Placa = "OLD", Modelo = "OldM", AreaId =3 };
 var request = new AtualizarMotoRequest { Placa = "NEW", Modelo = "NewM", AreaId =4 };

 mockRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
 mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Moto>())).ReturnsAsync((Moto m) => m);

 var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.UpdateAsync(7, request);

 Assert.NotNull(result);
 Assert.Equal("NEW", result.Placa);
 Assert.Equal("NewM", result.Modelo);
 Assert.Equal(4, result.AreaId);
 }
 }
}
