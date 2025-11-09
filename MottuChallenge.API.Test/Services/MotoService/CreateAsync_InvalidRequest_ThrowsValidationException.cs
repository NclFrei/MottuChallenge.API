using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Moq;
using Xunit;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;
using AutoMapper;

namespace MottuChallenge.API.Test.Services.MotoTests
{
 public class CreateAsync_InvalidRequest_ThrowsValidationException
 {
 [Fact]
 public async Task Test()
 {
 var mockRepo = new Mock<IMotoRepository>();
 var mockValidator = new Mock<IValidator<MotoRequest>>();
 var mockMapper = new Mock<IMapper>();

 var request = new MotoRequest { Placa = "", Modelo = "X", AreaId =1 };
 var failures = new List<FluentValidation.Results.ValidationFailure>
 {
 new FluentValidation.Results.ValidationFailure("Placa", "Placa é obrigatória")
 };
 var validationResult = new FluentValidation.Results.ValidationResult(failures);

 mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

 var service = new MotoService(mockRepo.Object, mockMapper.Object, mockValidator.Object);

 await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.CreateAsync(request));
 }
 }
}
