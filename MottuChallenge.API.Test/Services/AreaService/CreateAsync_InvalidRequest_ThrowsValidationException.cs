using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Moq;
using Xunit;
using AutoMapper;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Test.Services.AreaTests
{
 public class CreateAsync_InvalidRequest_ThrowsValidationException
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IAreaRepository>();
 var mockValidator = new Mock<IValidator<AreaRequest>>();
 var mockMapper = new Mock<IMapper>();

 var request = new AreaRequest { Nome = "", PatioId =1 };
 var failures = new List<FluentValidation.Results.ValidationFailure>
 {
 new FluentValidation.Results.ValidationFailure("Nome", "Nome é obrigatório")
 };
 var validationResult = new FluentValidation.Results.ValidationResult(failures);

 mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

 var service = new AreaService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.CreateAsync(request));
 }
 }
}
