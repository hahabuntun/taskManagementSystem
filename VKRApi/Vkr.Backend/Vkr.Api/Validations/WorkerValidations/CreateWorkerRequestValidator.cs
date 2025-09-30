using FluentValidation;
using Vkr.API.Contracts.WorkersControllerContracts;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Domain.Entities.Worker;

namespace Vkr.API.Validations.WorkerValidations;

public class CreateWorkerRequestValidator : AbstractValidator<CreateWorkerRequest>
{
    public CreateWorkerRequestValidator(IWorkerPositionsService workerPositionsService, IWorkersService workersService)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Поле 'Имя' обязательно")
            .MaximumLength(Workers.PropertyMaxLength).WithMessage($"Имя не должно превышать {Workers.PropertyMaxLength} символов");

        RuleFor(x => x.SecondName)
            .NotEmpty().WithMessage("Поле 'Фамилия' обязательно")
            .MaximumLength(Workers.PropertyMaxLength).WithMessage($"Фамилия не должна превышать {Workers.PropertyMaxLength} символов");

        RuleFor(x => x.ThirdName)
            .MaximumLength(Workers.PropertyMaxLength).WithMessage($"Отчество не должно превышать {Workers.PropertyMaxLength} символов");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Поле 'Email' обязательно")
            .EmailAddress().WithMessage("Невалидный адрес электронной почты")
            .MaximumLength(Workers.PropertyMaxLength).WithMessage($"Email не должен превышать {Workers.PropertyMaxLength} символов")
            .MustAsync(async (email, cancellationToken) => await workersService.IsEmailUnique(email))
            .WithMessage("Пользователь с таким Email уже существует");

        RuleFor(x => x.WorkerStatus)
            .Must(x => Enum.IsDefined(typeof(WorkerStatus), x))
            .WithMessage("Невалидный статус работника");

        RuleFor(x => x.WorkerPosition)
         .MustAsync(async (workerPosition, cancellationToken) =>
             await workerPositionsService.IsWorkerPositionExists(workerPosition))
         .WithMessage("Указанная должность не существует");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Поле 'Пароль' обязательно");
    }
}