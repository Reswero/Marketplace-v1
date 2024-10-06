using Marketplace.Common.Authorization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Marketplace.Common.Authorization.Attributes;

/// <summary>
/// Аттрибут авторизации по типу аккаунта
/// </summary>
/// <param name="type"></param>
[AttributeUsage(AttributeTargets.Method)]
public class AccountTypeAuthorizeAttribute(AccountType type) : Attribute, IAuthorizationFilter
{
    private readonly AccountType _requiredType = type;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var claimsExist = context.HttpContext.Items.TryGetValue(AuthorizationConsts.ClaimsKey, out var claimsObj);

        if (claimsExist is false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var claims = (AuthorizationClaims)claimsObj!;
        if (claims.Type == AccountType.None)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (claims.Type < _requiredType)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
