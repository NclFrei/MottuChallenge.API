using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Test.Services.UserTests
{
 public class GetAllUsersAsync_ReturnsPagedResponse
 {
 private Mock<IMapper> CreateMapperMock()
 {
 var mockMapper = new Mock<IMapper>();
 mockMapper.Setup(m => m.Map<System.Collections.Generic.List<UserResponse>>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is System.Collections.Generic.IEnumerable<User> list)
 return list.Select(u => new UserResponse { Id = u.Id, Nome = u.Nome, Email = u.Email }).ToList();
 return new System.Collections.Generic.List<UserResponse>();
 });
 return mockMapper;
 }

 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IUserRepository>();
 var mockMapper = CreateMapperMock();

 var users = new List<User>
 {
 new User { Id =1, Nome = "A" },
 new User { Id =2, Nome = "B" },
 new User { Id =3, Nome = "C" }
 };

 var queryable = users.AsQueryable();
 mockRepo.Setup(r => r.Query()).Returns(queryable);

 var service = new UserService(mockRepo.Object, mockMapper.Object);

 var response = await service.GetAllUsersAsync(page:1, limit:2);

 Assert.NotNull(response);
 Assert.Equal(2, response.Data.Count());
 Assert.Equal(3, response.Total);
 Assert.NotNull(response.Next);
 }
 }
}