using Marketplace.Common.Authorization.Extensions;
using Marketplace.Products.Api.Extensions;
using Marketplace.Products.Application;
using Marketplace.Products.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marketplace.Products.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddHeaderAuthorization();
            opt.IncludeXmlComments(Assembly.GetExecutingAssembly(), true);
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        app.Services.InitializeDatabase();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHeaderAuthorization();
        app.UseGlobalExceptionHandler();

        app.MapControllers();

        app.Run();
    }
}
