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

namespace MottuChallenge.API.Test.Services.PatioTests
{
 public class GetAllPatiosAsync_ReturnsPagedResponse
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IPatioRepository>();
 var mockValidator = new Mock<IValidator<MottuChallenge.API.Domain.Dtos.Request.PatioRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<List<PatioResponse>>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is IEnumerable<Patio> list)
 return list.Select(p => new PatioResponse { Id = p.Id, Nome = p.Nome, UserId = p.UserId }).ToList();
 return new List<PatioResponse>();
 });
 mockMapper.Setup(m => m.Map<PatioResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Patio p)
 return new PatioResponse { Id = p.Id, Nome = p.Nome, UserId = p.UserId };
 return null!;
 });

 var patios = new List<Patio>
 {
 new Patio { Id =1, Nome = "P1" },
 new Patio { Id =2, Nome = "P2" },
 new Patio { Id =3, Nome = "P3" }
 };

 var queryable = patios.AsQueryable();
 mockRepo.Setup(r => r.Query()).Returns(queryable);

 var service = new PatioService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var response = await service.GetAllPatiosAsync(page:1, limit:2);

 Assert.NotNull(response);
 Assert.Equal(2, response.Data.Count());
 Assert.Equal(3, response.Total);
 Assert.NotNull(response.Next);
 }
 }
}