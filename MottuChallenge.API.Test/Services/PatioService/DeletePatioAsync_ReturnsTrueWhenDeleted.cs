using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Request;

namespace MottuChallenge.API.Test.Services.PatioTests
{
 public class DeletePatioAsync_ReturnsTrueWhenDeleted
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IPatioRepository>();
 var mockValidator = new Mock<IValidator<PatioRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockRepo.Setup(r => r.DeleteAsync(3)).ReturnsAsync(true);

 var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.DeletePatioAsync(3);

 Assert.True(result);
 }
 }
}