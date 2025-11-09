using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.AreaTests
{
 public class CreateAsync_ValidRequest_ReturnsAreaResponse
 {
 private Mock<IMapper> CreateMapperMock()
 {
 var mockMapper = new Mock<IMapper>();
 mockMapper.Setup(m => m.Map<Area>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is AreaRequest ar)
 return new Area { Nome = ar.Nome, PatioId = ar.PatioId ??0 };
 return src as Area ?? new Area();
 });
 mockMapper.Setup(m => m.Map<AreaResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Area a)
 return new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId };
 return null!;
 });
 return mockMapper;
 }

 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IAreaRepository>();
 var mockValidator = new Mock<IValidator<AreaRequest>>();
 var mockMapper = CreateMapperMock();

 var request = new AreaRequest { Nome = "A1", PatioId =1 };
 var area = new Area { Id =1, Nome = "A1", PatioId =1 };

 mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
 mockRepo.Setup(r => r.CreateAsync(It.IsAny<Area>())).ReturnsAsync(area);

 var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.CreateAsync(request);

 Assert.NotNull(result);
 Assert.Equal(area.Id, result.Id);
 Assert.Equal(area.Nome, result.Nome);
 }
 }
}
