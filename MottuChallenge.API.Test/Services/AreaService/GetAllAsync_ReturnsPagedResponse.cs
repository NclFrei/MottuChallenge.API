using System.Linq;
using System.Collections.Generic;
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
public class GetAllAsync_ReturnsPagedResponse
{
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IAreaRepository>();
 var mockValidator = new Mock<IValidator<MottuChallenge.API.Domain.Dtos.Request.AreaRequest>>();
 var mockMapper = new Mock<IMapper>();

 // mapper setups
 mockMapper.Setup(m => m.Map<List<AreaResponse>>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is IEnumerable<Area> list)
 return list.Select(a => new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId }).ToList();
 return new List<AreaResponse>();
 });
 mockMapper.Setup(m => m.Map<AreaResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Area a)
 return new AreaResponse { Id = a.Id, Nome = a.Nome, PatioId = a.PatioId };
 return null!;
 });

 var areas = new List<Area>
 {
 new Area { Id =1, Nome = "A1", PatioId =1 },
 new Area { Id =2, Nome = "A2", PatioId =1 },
 new Area { Id =3, Nome = "A3", PatioId =2 }
 };

 var queryable = areas.AsQueryable();
 mockRepo.Setup(r => r.Query()).Returns(queryable);

 var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var response = await service.GetAllAsync(null, null, page:1, limit:2);

 Assert.NotNull(response);
 Assert.Equal(2, response.Data.Count());
 Assert.Equal(3, response.Total);
 Assert.NotNull(response.Next);
 }
}
}
