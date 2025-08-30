using FluentValidation;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Domain.Validator;

public class AreaCreateRequestValidator : AbstractValidator<AreaRequest>
{
    public AreaCreateRequestValidator()
    {
        RuleFor(a => a.Nome)
            .NotEmpty().WithMessage("O nome da área é obrigatório.")
            .MinimumLength(3).WithMessage("O nome da área deve ter pelo menos 3 caracteres.");

        RuleFor(a => a.PatioId)
            .GreaterThan(0).WithMessage("O ID do pátio deve ser maior que zero.");
        
    }
}