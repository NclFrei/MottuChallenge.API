using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.AreaTests
{
    public class UpdateAsync_NonExistingArea_ReturnsNull
    {
        [Fact]
        public async Task Test()
        {
            var mockRepo = new Mock<IAreaRepository>();
            var mockValidator = new Mock<IValidator<AreaRequest>>();
            var mockMapper = new Mock<IMapper>();

            var request = new AtualizarAreaRequest { Nome = "X" };
            mockRepo.Setup(r => r.GetByIdAsync(20)).ReturnsAsync((MottuChallenge.API.Domain.Models.Area?)null);

            var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

            var result = await service.UpdateAsync(20, request);

            Assert.Null(result);
        }
    }
}
