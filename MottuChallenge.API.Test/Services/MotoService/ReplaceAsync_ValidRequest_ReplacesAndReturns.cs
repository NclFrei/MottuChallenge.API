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
 public class ReplaceAsync_ValidRequest_ReplacesAndReturns
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IMotoRepository>();
 var mockValidator = new Mock<IValidator<MotoRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<MotoRequest, Moto>(It.IsAny<MotoRequest>(), It.IsAny<Moto>())).Returns((MotoRequest src, Moto dest) =>
 {
 if (!string.IsNullOrWhiteSpace(src.Placa)) dest.Placa = src.Placa!;
 if (!string.IsNullOrWhiteSpace(src.Modelo)) dest.Modelo = src.Modelo!;
 if (src.AreaId.HasValue) dest.AreaId = src.AreaId.Value;
 return dest;
 });
 mockMapper.Setup(m => m.Map(It.IsAny<object>(), It.IsAny<object>())).Returns((object src, object dest) =>
 {
 if (src is MotoRequest mr && dest is Moto m)
 {
 if (!string.IsNullOrWhiteSpace(mr.Placa)) m.Placa = mr.Placa!;
 if (!string.IsNullOrWhiteSpace(mr.Modelo)) m.Modelo = mr.Modelo!;
 if (mr.AreaId.HasValue) m.AreaId = mr.AreaId.Value;
 return (object)m;
 }
 return dest;
 });
 mockMapper.Setup(m => m.Map<MotoResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Moto m)
 return new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo, AreaId = m.AreaId };
 return null!;
 });

 var request = new MotoRequest { Placa = "R1", Modelo = "RM", AreaId =9 };
 var existing = new Moto { Id =8, Placa = "Old", Modelo = "OM", AreaId =1 };

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
 }
}
