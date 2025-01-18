using Marketplace.Products.Application.Common.Interfaces;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.CheckProductsExistence;

internal class CheckProductsExistenceQueryHandler(IProductsRepository repository)
    : IRequestHandler<CheckProductsExistenceQuery, List<int>>
{
    private readonly IProductsRepository _repository = repository;

    public async Task<List<int>> Handle(CheckProductsExistenceQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAsync(request.Ids, cancellationToken);
        return products.Select(p => p.Id).ToList();
    }
}
