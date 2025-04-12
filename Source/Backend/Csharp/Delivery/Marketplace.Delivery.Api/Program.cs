using Marketplace.Delivery.Infrastructure;
using Marketplace.Delivery.Infrastructure.Deliveries.Services;

namespace Marketplace.Delivery.Api;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGrpc();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();
        app.Services.InitializeDatabase();

        app.MapGrpcService<DeliveryGrpcService>();

        app.Run();
    }
}