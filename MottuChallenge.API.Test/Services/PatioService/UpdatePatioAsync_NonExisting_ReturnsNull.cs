using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.PatioTests
{
 public class UpdatePatioAsync_NonExisting_ReturnsNull
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IPatioRepository>();
 var mockValidator = new Mock<IValidator<PatioRequest>>();
 var mockMapper = new Mock<IMapper>();

 var request = new AtualizarPatioRequest { Nome = "X" };
 mockRepo.Setup(r => r.GetByIdAsync(50)).ReturnsAsync((MottuChallenge.API.Domain.Models.Patio?)null);

 var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.UpdatePatioAsync(50, request);

 Assert.Null(result);
 }
 }
}