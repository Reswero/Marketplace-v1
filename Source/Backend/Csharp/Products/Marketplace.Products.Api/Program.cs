using Marketplace.Common.Authorization.Extensions;
using Marketplace.Products.Api.Extensions;
using Marketplace.Products.Application;
using Marketplace.Products.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Marketplace.Products.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddHeaderAuthorization();
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
