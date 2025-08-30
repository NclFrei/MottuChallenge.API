using FluentValidation;
using MottuChallenge.API.Domain.Dtos.Request;

namespace MottuChallenge.API.Domain.Validator;


public class EnderecoCreateRequestValidator : AbstractValidator<EnderecoRequest>
{
    public EnderecoCreateRequestValidator()
    {
        RuleFor(e => e.Rua).NotEmpty().WithMessage("O nome da rua é obrigatório.");
        RuleFor(e => e.Numero).GreaterThan(0).WithMessage("O número do endereço é obrigatório.");
        RuleFor(e => e.Bairro).NotEmpty().WithMessage("O bairro é obrigatório.");
        RuleFor(e => e.Cidade).NotEmpty().WithMessage("A cidade é obrigatória.");
        RuleFor(e => e.Estado).NotEmpty().WithMessage("O estado é obrigatório.");
        RuleFor(e => e.Cep)
            .NotEmpty().WithMessage("O CEP é obrigatório.")
            .Matches(@"^\d{5}-?\d{3}$").WithMessage("CEP inválido. Use o formato 12345-678 ou 12345678.");
    }
}