using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Integrations.Products;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marketplace.Orders.Infrastructure.Integrations.Products.Services;

/// <summary>
/// Клиент к сервису товаров
/// </summary>
internal class ProductsServiceClient(HttpClient httpClient)
    : IProductsServiceClient
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly HttpClient _httpClient = httpClient;

    static ProductsServiceClient()
    {
        _options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    /// <inheritdoc/>
    public async Task<List<Product>> GetProductsByIdsAsync(int[] ids, CancellationToken cancellationToken = default)
    {
        QueryBuilder builder = [];
        foreach (int id in ids)
        {
            builder.Add("Ids", id.ToString());
        }
        var query = builder.ToQueryString();

        var response = await _httpClient.GetAsync($"internal/v1/products/by-ids{query}", cancellationToken);
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<Product>>(_options, cancellationToken);
        // TODO: null check

        return products!;
    }
}
