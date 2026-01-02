using Microsoft.AspNetCore.Http;

namespace WEB_253551_URBANOVICH.UI.Services.Authentication;

public interface IAuthService
{
    /// <summary>
    /// Регистрация пользователя на сервере аутентификации (Keycloak).
    /// </summary>
    /// <param name="email">Email нового пользователя</param>
    /// <param name="password">Пароль</param>
    /// <param name="avatar">Файл аватара (необязательно)</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>
    /// Result - признак успешной регистрации.
    /// ErrorMessage - сообщение об ошибке (если есть).
    /// </returns>
    Task<(bool Result, string ErrorMessage)> RegisterUserAsync(
        string email,
        string password,
        IFormFile? avatar,
        CancellationToken ct = default);
}
