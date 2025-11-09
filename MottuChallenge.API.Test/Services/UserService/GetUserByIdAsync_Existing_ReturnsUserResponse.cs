using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Dtos.Request;

namespace MottuChallenge.API.Test.Services.UserTests
{
 public class GetUserByIdAsync_Existing_ReturnsUserResponse
 {
 private Mock<IMapper> CreateMapperMock()
 {
 var mockMapper = new Mock<IMapper>();

 mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is User u)
 return new UserResponse { Id = u.Id, Nome = u.Nome, Email = u.Email };
 return null!;
 });

 mockMapper.Setup(m => m.Map<System.Collections.Generic.List<UserResponse>>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is System.Collections.Generic.IEnumerable<User> list)
 return System.Linq.Enumerable.Select(list, u => new UserResponse { Id = u.Id, Nome = u.Nome, Email = u.Email }) as System.Collections.Generic.List<UserResponse> ?? new System.Collections.Generic.List<UserResponse>();
 return new System.Collections.Generic.List<UserResponse>();
 });

 mockMapper.Setup(m => m.Map<AtualizarUserRequest, User>(It.IsAny<AtualizarUserRequest>(), It.IsAny<User>())).Returns((AtualizarUserRequest src, User dest) =>
 {
 if (!string.IsNullOrWhiteSpace(src.Nome)) dest.Nome = src.Nome!;
 if (!string.IsNullOrWhiteSpace(src.Email)) dest.Email = src.Email!;
 return dest;
 });

 return mockMapper;
 }

 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IUserRepository>();
 var mockMapper = CreateMapperMock();

 var user = new User { Id =2, Nome = "U", Email = "u@x" };
 mockRepo.Setup(r => r.BuscarPorIdAsync(2)).ReturnsAsync(user);

 var service = new UserService(mockRepo.Object, mockMapper.Object);

 var result = await service.GetUserByIdAsync(2);

 Assert.NotNull(result);
 Assert.Equal(user.Id, result.Id);
 }
 }
}