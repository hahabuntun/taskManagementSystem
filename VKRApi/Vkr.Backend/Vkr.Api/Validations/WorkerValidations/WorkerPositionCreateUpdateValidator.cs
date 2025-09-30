using FluentValidation;
using Vkr.API.Contracts.WorkerPositionsControllerContracts;
using Vkr.Domain.Entities.Worker;

namespace Vkr.API.Validations.WorkerValidations;

public class WorkerPositionCreateUpdateValidator : AbstractValidator<WorkerPositionCreateUpdateRequest>
{
    public WorkerPositionCreateUpdateValidator()
    {
        RuleFor(r => r.Title)
            .NotEmpty()
            .WithMessage("Название должности не должно быть пустым")
            .MaximumLength(WorkerPosition.MaxTitleLength)
            .WithMessage($"Название должности не должно превышать {WorkerPosition.MaxTitleLength} символов");
    }
}