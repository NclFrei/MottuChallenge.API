using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.MotoTests
{
 public class UpdateAsync_NonExistingMoto_ReturnsNull
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IMotoRepository>();
 var mockValidator = new Mock<IValidator<MotoRequest>>();
 var mockMapper = new Mock<IMapper>();

 var request = new AtualizarMotoRequest { Placa = "X" };
 mockRepo.Setup(r => r.GetByIdAsync(20)).ReturnsAsync((MottuChallenge.API.Domain.Models.Moto?)null);

 var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.UpdateAsync(20, request);

 Assert.Null(result);
 }
 }
}
