using FluentValidation;
using Vkr.API.Contracts.AuthControllerContracts;

namespace Vkr.API.Validations.AuthValidations
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Поле пароль обязательно");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Поле email обязательно");
        }
    }
}
