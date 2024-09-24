using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Sellers.Queries.GetSeller;

/// <summary>
/// Получение профиля продавца
/// </summary>
/// <param name="repository"></param>
internal class GetSellerQueryHandler(ISellersRepository repository) : IRequestHandler<GetSellerQuery, Seller>
{
    private readonly ISellersRepository _repository = repository;

    public async Task<Seller> Handle(GetSellerQuery request, CancellationToken cancellationToken)
    {
        var seller = await _repository.GetAsync(request.AccountId, cancellationToken);
        return seller;
    }
}
