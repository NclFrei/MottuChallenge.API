using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.AreaTests
{
 public class GetByIdAsync_NonExistingId_ReturnsNull
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IAreaRepository>();
 var mockValidator = new Mock<IValidator<MottuChallenge.API.Domain.Dtos.Request.AreaRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((MottuChallenge.API.Domain.Models.Area?)null);

 var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.GetByIdAsync(10);

 Assert.Null(result);
 }
 }
}
