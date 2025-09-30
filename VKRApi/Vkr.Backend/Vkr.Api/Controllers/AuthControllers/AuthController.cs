using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Vkr.API.Contracts.AuthControllerContracts;
using Vkr.Application.Interfaces.Services.WorkerServices;

namespace Vkr.API.Controllers.AuthControllers;

/// <summary>
/// Авторизация пользователей
/// </summary>
[Route("api/auth")]
[ApiController]
public class AuthController(IWorkersService workersService) : ControllerBase
{
    /// <summary>
    /// Выполняет вход пользователя в систему и возвращает токен авторизации.
    /// </summary>
    /// <param name="request">Объект с данными для входа (email и пароль пользователя).</param>
    /// <param name="validator">Сервис для проверки валидности данных для входа.</param>
    /// <returns>
    /// Возвращает результат операции входа:
    /// <list type="bullet">
    /// <item><description>HTTP 200 (Ok) с токеном авторизации, если вход успешен.</description></item>
    /// <item><description>HTTP 400 (Bad Request), если данные невалидны (например, неверный формат email или пароль).</description></item>
    /// <item><description>HTTP 401 (Unauthorized), если email или пароль не совпадают с данными в системе.</description></item>
    /// </list>
    /// </returns>
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserRequest request, [FromServices] IValidator<LoginUserRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(error => new { error.PropertyName, error.ErrorMessage })
            });
        }

        var response = await workersService.Login(request.Email, request.Password);

        if(string.IsNullOrEmpty(response))
        {
            return Unauthorized(new { Error = "Пользователя с таким email или паролем не сущетсвует"});
        }

        return Ok(new { Token = response});
    }
}