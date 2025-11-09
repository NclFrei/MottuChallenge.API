using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Test.Services.AreaTests
{
 public class ReplaceAsync_ValidRequest_ReplacesAndReturns
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IAreaRepository>();
 var mockValidator = new Mock<IValidator<AreaRequest>>();
 var mockMapper = new Mock<IMapper>();

 // generic Map<AreaRequest, Area>
 mockMapper.Setup(m => m.Map<AreaRequest, Area>(It.IsAny<AreaRequest>(), It.IsAny<Area>())).Returns((AreaRequest src, Area dest) =>
 {
 if (!string.IsNullOrWhiteSpace(src.Nome)) dest.Nome = src.Nome!;
 if (src.PatioId.HasValue) dest.PatioId = src.PatioId.Value;
 return dest;
 });
 // non-generic fallback to ensure Map(request, existing) works
 mockMapper.Setup(m => m.Map(It.IsAny<object>(), It.IsAny<object>())).Returns((object src, object dest) =>
 {
 if (src is AreaRequest ar && dest is Area a)
 {
 if (!string.IsNullOrWhiteSpace(ar.Nome)) a.Nome = ar.Nome!;
 if (ar.PatioId.HasValue) a.PatioId = ar.PatioId.Value;
 return (object)a;
 }
 return dest;
 });

 mockMapper.Setup(m => m.Map<AreaResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Area a)
 return new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId };
 return null!;
 });

 var request = new AreaRequest { Nome = "R1", PatioId =9 };
 var existing = new Area { Id =8, Nome = "Old", PatioId =1 };

 mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
 mockRepo.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(existing);
 mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Area>())).ReturnsAsync((Area a) => a);

 var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.ReplaceAsync(8, request);

 Assert.NotNull(result);
 Assert.Equal(8, result.Id);
 Assert.Equal("R1", result.Nome);
 Assert.Equal(9, result.PatioId);
 }
 }
}
