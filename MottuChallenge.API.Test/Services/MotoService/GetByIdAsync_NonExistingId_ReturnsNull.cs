using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Request;

namespace MottuChallenge.API.Test.Services.MotoTests
{
 public class GetByIdAsync_NonExistingId_ReturnsNull
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IMotoRepository>();
 var mockValidator = new Mock<IValidator<MottuChallenge.API.Domain.Dtos.Request.MotoRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((MottuChallenge.API.Domain.Models.Moto?)null);

 var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.GetByIdAsync(10);

 Assert.Null(result);
 }
 }
}
