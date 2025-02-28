using Marketplace.Common.Authorization.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;
using Marketplace.Orders.Application;
using Marketplace.Orders.Infrastructure;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Marketplace.Orders.Api;

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
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddHeaderAuthorization();
            options.IncludeXmlComments(Assembly.GetExecutingAssembly(), true);
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

        app.MapControllers();

        app.Run();
    }
}
