using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.AreaTests
{
 public class DeleteAsync_ReturnsFalseWhenNotFound
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IAreaRepository>();
 var mockValidator = new Mock<IValidator<MottuChallenge.API.Domain.Dtos.Request.AreaRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockRepo.Setup(r => r.DeleteAsync(33)).ReturnsAsync(false);

 var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.DeleteAsync(33);

 Assert.False(result);
 }
 }
}
