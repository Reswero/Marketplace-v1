using Marketplace.Common.Authorization.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Marketplace.Common.Authorization.Attributes;

/// <summary>
/// Аттрибут авторизации по типу аккаунта
/// </summary>
/// <param name="type"></param>
[AttributeUsage(AttributeTargets.Method)]
public class AccountTypeAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly AccountType _requiredType;

    public AccountTypeAuthorizeAttribute(params AccountType[] types)
    {
        foreach (var type in types)
        {
            _requiredType |= type;
        }
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var claimsExist = context.HttpContext.Items.TryGetValue(AuthorizationConsts.ClaimsKey, out var claimsObj);

        if (claimsExist is false)
        {
            context.Result = AuthorizationConsts.UnauthorizedResultResponse;
            return;
        }

        var claims = (AuthorizationClaims)claimsObj!;
        if (claims.Type == AccountType.None)
        {
            context.Result = AuthorizationConsts.UnauthorizedResultResponse;
            return;
        }

        if (_requiredType.HasFlag(claims.Type) is false)
        {
            context.Result = AuthorizationConsts.ForbidResultResponse;
            return;
        }
    }
}
