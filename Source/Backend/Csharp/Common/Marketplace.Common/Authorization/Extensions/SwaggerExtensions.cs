using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Marketplace.Common.Authorization.Extensions;

/// <summary>
/// Расширения Swagger'а
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Добавляет заголовки для авторизационных данных
    /// </summary>
    /// <param name="options"></param>
    public static void AddHeaderAuthorization(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(AuthorizationConsts.IdHeader, new OpenApiSecurityScheme()
        {
            Name = AuthorizationConsts.IdHeader,
            In = ParameterLocation.Header
        });

        options.AddSecurityDefinition(AuthorizationConsts.TypeHeader, new OpenApiSecurityScheme()
        {
            Name = AuthorizationConsts.TypeHeader,
            In = ParameterLocation.Header
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = AuthorizationConsts.IdHeader,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = AuthorizationConsts.TypeHeader,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
    }
}
