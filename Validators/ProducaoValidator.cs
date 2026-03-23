using FluentValidation;
using OpMetrics.Core.DTOs.Requests;

namespace OpMetrics.Core.Validators;
public class CreateProducaoValidator : AbstractValidator<CreateProducaoRequest>
{
    public CreateProducaoValidator()
    {
        RuleFor(x => x.Linha)
            .NotEmpty().WithMessage("Linha é obrigatória")
            .MaximumLength(100).WithMessage("Linha deve ter no máximo 100 caracteres");

        RuleFor(x => x.Data)
            .NotEmpty().WithMessage("Data é obrigatória")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data não pode ser futura");

        RuleFor(x => x.Turno)
            .NotEmpty().WithMessage("Turno é obrigatório")
            .Must(t => t == "Manha" || t == "Tarde" || t == "Noite")
            .WithMessage("Turno deve ser: Manha, Tarde ou Noite.");

        RuleFor(x => x.MetaPecas)
            .GreaterThan(0).WithMessage("Meta de peças deve ser maior que zero.");

        RuleFor(x => x.PecasProduzidas)
            .GreaterThanOrEqualTo(0).WithMessage("Valor não pode ser negativo");

    }

    public CreateQualidadeValidator()
    {

    }
}
