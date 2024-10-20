using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Application.Sellers.ViewModels;
using MediatR;

namespace Marketplace.Users.Application.Sellers.Queries.GetSellerShortInfo;

/// <summary>
/// Получение краткой информации о продавце
/// </summary>
internal class GetSellerShortInfoQueryHandler(ISellersRepository repository)
    : IRequestHandler<GetSellerShortInfoQuery, SellerShortInfoVM>
{
    private readonly ISellersRepository _repository = repository;

    public async Task<SellerShortInfoVM> Handle(GetSellerShortInfoQuery request, CancellationToken cancellationToken)
    {
        var seller = await _repository.GetAsync(request.AccountId, cancellationToken);
        return new(seller.AccountId, seller.CompanyName);
    }
}
