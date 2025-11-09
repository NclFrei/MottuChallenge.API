using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Test.Services.UserTests
{
 public class AtualizarPerfilAsync_Existing_UpdatesAndReturns
 {
 private Mock<IMapper> CreateMapperMock()
 {
 var mockMapper = new Mock<IMapper>();
 mockMapper.Setup(m => m.Map<AtualizarUserRequest, User>(It.IsAny<AtualizarUserRequest>(), It.IsAny<User>())).Returns((AtualizarUserRequest src, User dest) =>
 {
 if (!string.IsNullOrWhiteSpace(src.Nome)) dest.Nome = src.Nome!;
 if (!string.IsNullOrWhiteSpace(src.Email)) dest.Email = src.Email!;
 return dest;
 });
 mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<object>())).Returns((object src) =>
 {
 if (src is User u)
 return new UserResponse { Id = u.Id, Nome = u.Nome, Email = u.Email };
 return null!;
 });
 return mockMapper;
 }

 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IUserRepository>();
 var mockMapper = CreateMapperMock();

 var existing = new User { Id =12, Nome = "Old", Email = "old@x", Password = "" };
 var request = new AtualizarUserRequest { Nome = "New", Email = "new@x", Password = "pass" };

 mockRepo.Setup(r => r.BuscarPorIdAsync(12)).ReturnsAsync(existing);
 mockRepo.Setup(r => r.AtualizarAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);

 var service = new UserService(mockRepo.Object, mockMapper.Object);

 var result = await service.AtualizarPerfilAsync(12, request);

 Assert.NotNull(result);
 Assert.Equal("New", result.Nome);
 Assert.Equal("new@x", result.Email);
 }
 }
}