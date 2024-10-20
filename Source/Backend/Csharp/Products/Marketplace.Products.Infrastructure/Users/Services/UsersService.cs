using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Users.Models;
using System.Net;
using System.Text.Json;

namespace Marketplace.Products.Infrastructure.Users.Services;

/// <summary>
/// Взаимодействие с сервисом пользователей
/// </summary>
internal class UsersService(HttpClient httpClient)
    : IUsersService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <inheritdoc/>
    public async Task<Seller> GetSellerInfoAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/v1/sellers/{id}/info", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ObjectNotFoundException(typeof(Seller), id);
        }

        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var seller = JsonSerializer.Deserialize<Seller>(stream, _options)!;

        return seller;
    }
}
