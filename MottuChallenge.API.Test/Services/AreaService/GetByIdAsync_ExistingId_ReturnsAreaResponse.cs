using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Test.Services.AreaTests
{
 public class GetByIdAsync_ExistingId_ReturnsAreaResponse
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IAreaRepository>();
 var mockValidator = new Mock<IValidator<MottuChallenge.API.Domain.Dtos.Request.AreaRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<AreaResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Area a)
 return new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId };
 return null!;
 });

 var area = new Area { Id =5, Nome = "Area5", PatioId =2 };
 mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(area);

 var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.GetByIdAsync(5);

 Assert.NotNull(result);
 Assert.Equal(area.Id, result.Id);
 }
 }
}
