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
using MottuChallenge.API.Domain.Dtos.Request;

namespace MottuChallenge.API.Test.Services.MotoTests
{
 public class GetAllAsync_ReturnsPagedResponse
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IMotoRepository>();
 var mockValidator = new Mock<IValidator<MottuChallenge.API.Domain.Dtos.Request.MotoRequest>>();
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<List<MotoResponse>>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is IEnumerable<Moto> list)
 return list.Select(m => new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo, AreaId = m.AreaId }).ToList();
 return new List<MotoResponse>();
 });
 mockMapper.Setup(m => m.Map<MotoResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is Moto m)
 return new MotoResponse { Id = m.Id, Placa = m.Placa, Modelo = m.Modelo, AreaId = m.AreaId };
 return null!;
 });

 var motos = new List<Moto>
 {
 new Moto { Id =1, Placa = "A1", Modelo = "M1", AreaId =1 },
 new Moto { Id =2, Placa = "A2", Modelo = "M1", AreaId =1 },
 new Moto { Id =3, Placa = "A3", Modelo = "M2", AreaId =2 }
 };

 var queryable = motos.AsQueryable();
 mockRepo.Setup(r => r.Query()).Returns(queryable);

 var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 var response = await service.GetAllAsync(null, null, page:1, limit:2);

 Assert.NotNull(response);
 Assert.Equal(2, response.Data.Count());
 Assert.Equal(3, response.Total);
 Assert.NotNull(response.Next);
 }
 }
}
