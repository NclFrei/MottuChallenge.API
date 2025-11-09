using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.PatioTests
{
 public class GetPatioByUserIdAsync_NonExisting_ReturnsNull
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IPatioRepository>();
 var mockValidator = new Mock<IValidator<MottuChallenge.API.Domain.Dtos.Request.PatioRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((MottuChallenge.API.Domain.Models.Patio?)null);

 var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.GetPatioByUserIdAsync(99);

 Assert.Null(result);
 }
 }
}