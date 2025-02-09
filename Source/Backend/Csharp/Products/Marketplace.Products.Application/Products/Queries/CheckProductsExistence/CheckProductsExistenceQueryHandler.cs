using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.CheckProductsExistence;

internal class CheckProductsExistenceQueryHandler(IProductsRepository repository)
    : IRequestHandler<CheckProductsExistenceQuery, ExistingProductsVM>
{
    private readonly IProductsRepository _repository = repository;

    public async Task<ExistingProductsVM> Handle(CheckProductsExistenceQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAsync(request.Ids, cancellationToken);
        return new(products.Select(p => p.Id).ToArray());
    }
}
