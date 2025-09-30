using FluentValidation;
using Vkr.API.Contracts.WorkersManagmentContracts;

namespace Vkr.API.Validations.WorkerValidations;

public class CreateWorkersConnectionValidator : AbstractValidator<CreateManagerToEmployeeRequest>
{
    public CreateWorkersConnectionValidator()
    {
        
    }
}