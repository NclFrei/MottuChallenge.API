using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Test.Services.UserTests
{
 public class DeleteAsync_Existing_ReturnsTrue
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
 return mockMapper;
 }

 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IUserRepository>();
 var mockMapper = CreateMapperMock();

 var user = new User { Id =3 };
 mockRepo.Setup(r => r.BuscarPorIdAsync(3)).ReturnsAsync(user);
 mockRepo.Setup(r => r.DeleteAsync(user)).ReturnsAsync(true);

 var service = new UserService(mockRepo.Object, mockMapper.Object);

 var result = await service.DeleteAsync(3);

 Assert.True(result);
 }
 }
}