
using FluentValidation;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Domain.Entities.Worker;

namespace Vkr.API.Validations.WorkerValidations;

public class UpdateWorkerRequestValidator : AbstractValidator<UpdateWorkerRequest>
{
    public UpdateWorkerRequestValidator(IWorkerPositionsService workerPositionsService, IWorkersService workersService)
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
            .MustAsync(async (request, email, cancellationToken) => await workersService.IsEmailUniqueForUpdate(request.Id, email))
            .WithMessage("Пользователь с таким Email уже существует");
        RuleFor(x => x.WorkerPosition)
            .MustAsync(async (workerPosition, cancellationToken) =>
                await workerPositionsService.IsWorkerPositionExists(workerPosition))
            .WithMessage("Указанная должность не существует");

        RuleFor(x => x.Password)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.Password))
            .WithMessage("Поле 'Пароль' не может быть пустым, если указано");

        RuleFor(x => x.Phone)
            .MaximumLength(Workers.PropertyMaxLength).WithMessage($"Телефон не должен превышать {Workers.PropertyMaxLength} символов");
    }
}