using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Integrations.Products;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http.Json;

namespace Marketplace.Orders.Infrastructure.Integrations.Products.Services;

/// <summary>
/// Клиент к сервису товаров
/// </summary>
internal class ProductsServiceClient(HttpClient httpClient)
    : IProductsServiceClient
{
    private readonly HttpClient _httpClient = httpClient;

    /// <inheritdoc/>
    public async Task<List<Product>> GetProductsByIdsAsync(int[] ids, CancellationToken cancellationToken = default)
    {
        QueryBuilder builder = [];
        foreach (int id in ids)
        {
            builder.Add("ids", id.ToString());
        }
        var query = builder.ToQueryString();

        var response = await _httpClient.GetAsync($"/internal/v1/products/by-ids{query}", cancellationToken);
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<Product>>(cancellationToken);
        // TODO: null check

        return products!;
    }
}
