using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Infrastructure.Integrations.Favorites.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace Marketplace.Products.Infrastructure.Integrations.Favorites;

/// <summary>
///  Взаимодействие с сервисом избранного
/// </summary>
/// <param name="httpClient"></param>
internal class FavoritesService(HttpClient httpClient)
    : IFavoritesService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<HashSet<int>> CheckProductsInFavoritesAsync(int customerId, List<int> idsToCheck,
        CancellationToken cancellationToken = default)
    {
        CheckFavoritesProductsDto dto = new(idsToCheck);
        var response = await _httpClient.PostAsJsonAsync($"v1/internal/customers/{customerId}/in-favorites", dto,
            _options, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var list = JsonSerializer.Deserialize<FavoritesListDto>(stream, _options)!;

        return list.ProductIds;
    }
}
