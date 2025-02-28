using Marketplace.Common.Authorization.Extensions;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.Identity.User;

/// <summary>
/// Провайдер идентификационных данных пользователя
/// </summary>
internal class HttpUserIdentityProvider : IUserIdentityProvider
{
    public HttpUserIdentityProvider(IHttpContextAccessor accessor)
    {
        var claims = accessor.HttpContext?.GetClaims();
        Id = claims?.Id != 0 ? claims?.Id : null;
    }

    /// <inheritdoc/>
    public int? Id { get; private set; }
}
