using Marketplace.Common.Authorization.Extensions;
using Marketplace.Users.Api.Extensions;
using Marketplace.Users.Application;
using Marketplace.Users.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Marketplace.Users.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseGlobalExceptionHandler();
            app.UseHeaderAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
