using FluentValidation;
using MottuChallenge.API.Domain.Dtos.Request;

namespace MottuChallenge.API.Domain.Validator;

public class MotoCreateRequestValidator : AbstractValidator<MotoRequest>
{

    public MotoCreateRequestValidator()
    {
        RuleFor(m => m.Placa)
            .NotEmpty().WithMessage("A placa da moto é obrigatória.")
            .Matches(@"^[A-Z]{3}-\d{4}$").WithMessage("Placa inválida. Use o formato ABC-1234.");

        RuleFor(m => m.Modelo)
            .NotEmpty().WithMessage("O modelo da moto é obrigatório.");

        RuleFor(m => m.AreaId)
            .NotNull().WithMessage("O ID da área é obrigatório.")
            .GreaterThan(0).WithMessage("O ID da área deve ser maior que zero.");
    }
}