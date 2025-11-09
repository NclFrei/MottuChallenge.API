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
 public class UpdateAsync_ExistingArea_UpdatesAndReturns
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IAreaRepository>();
 var mockValidator = new Mock<IValidator<AreaRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<AtualizarAreaRequest, Area>(It.IsAny<AtualizarAreaRequest>(), It.IsAny<Area>())).Returns((AtualizarAreaRequest src, Area dest) =>
 {
 if (!string.IsNullOrWhiteSpace(src.Nome)) dest.Nome = src.Nome!;
 if (src.PatioId.HasValue) dest.PatioId = src.PatioId.Value;
 return dest;
 });
 mockMapper.Setup(m => m.Map<AreaResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Area a)
 return new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId };
 return null!;
 });

 var existing = new Area { Id =7, Nome = "Old", PatioId =3 };
 var request = new AtualizarAreaRequest { Nome = "New", PatioId =4 };

 mockRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
 mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Area>())).ReturnsAsync((Area a) => a);

 var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var result = await service.UpdateAsync(7, request);

 Assert.NotNull(result);
 Assert.Equal("New", result.Nome);
 Assert.Equal(4, result.PatioId);
 }
 }
}
