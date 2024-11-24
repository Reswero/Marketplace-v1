using Marketplace.Common.Authorization.Extensions;
using Marketplace.Products.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Products.Infrastructure.Common.Services;

/// <summary>
/// Провайдер идентификационных данных пользователя
/// </summary>
internal class HttpUserIdentityProvider : IUserIdentityProvider
{
    public HttpUserIdentityProvider(HttpContext httpContext)
    {
        var claims = httpContext.GetClaims();
        Id = claims.Id != 0 ? claims.Id : null;
    }

    /// <inheritdoc/>
    public int? Id { get; private set; }
}
