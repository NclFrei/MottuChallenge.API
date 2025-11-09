using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Test.Services.PatioTests
{
 public class GetPatioByIdAsync_Existing_ReturnsPatioResponse
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IPatioRepository>();
 var mockValidator = new Mock<IValidator<MottuChallenge.API.Domain.Dtos.Request.PatioRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<PatioResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Patio p)
 return new PatioResponse { Id = p.Id, Nome = p.Nome, UserId = p.UserId };
 return null!;
 });

 var patio = new Patio { Id =11, Nome = "IdPatio" };
 mockRepo.Setup(r => r.GetByIdAsync(11)).ReturnsAsync(patio);

 var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.GetPatioByIdAsync(11);

 Assert.NotNull(result);
 Assert.Equal(patio.Id, result.Id);
 }
 }
}