using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.UserTests
{
 public class GetUserByIdAsync_NonExisting_ReturnsNull
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IUserRepository>();
 var mockMapper = new Mock<IMapper>();

 mockRepo.Setup(r => r.BuscarPorIdAsync(99)).ReturnsAsync((MottuChallenge.API.Domain.Models.User?)null);

 var service = new UserService(mockRepo.Object, mockMapper.Object);

 var result = await service.GetUserByIdAsync(99);

 Assert.Null(result);
 }
 }
}