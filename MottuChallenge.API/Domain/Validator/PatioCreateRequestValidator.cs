using FluentValidation;
using MottuChallenge.API.Domain.Dtos.Request;

namespace MottuChallenge.API.Domain.Validator;


public class PatioCreateRequestValidator : AbstractValidator<PatioRequest>
{
    public PatioCreateRequestValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome do pátio é obrigatório.")
            .MinimumLength(3).WithMessage("O nome do pátio deve ter pelo menos 3 caracteres.");

        RuleFor(p => p.UserId)
            .GreaterThan(0).WithMessage("O UserId deve ser maior que zero.");

        RuleFor(p => p.Endereco)
            .NotNull().WithMessage("O endereço é obrigatório.")
            .SetValidator(new EnderecoCreateRequestValidator()); 
    }
}