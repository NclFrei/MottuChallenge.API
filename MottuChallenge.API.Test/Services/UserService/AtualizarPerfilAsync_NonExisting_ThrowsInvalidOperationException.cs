using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.UserTests
{
 public class AtualizarPerfilAsync_NonExisting_ThrowsInvalidOperationException
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IUserRepository>();
 var mockMapper = new Mock<IMapper>();

 var request = new AtualizarUserRequest { Nome = "X" };
 mockRepo.Setup(r => r.BuscarPorIdAsync(99)).ReturnsAsync((MottuChallenge.API.Domain.Models.User?)null);

 var service = new UserService(mockRepo.Object, mockMapper.Object);

 await Assert.ThrowsAsync<System.InvalidOperationException>(() => service.AtualizarPerfilAsync(99, request));
 }
 }
}