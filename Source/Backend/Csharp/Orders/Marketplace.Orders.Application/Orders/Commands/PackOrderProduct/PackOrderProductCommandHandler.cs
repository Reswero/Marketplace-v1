using Marketplace.Common.Identity.User;
using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Exceptions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Domain;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Commands.PackOrderProduct;

/// <summary>
/// Установить товару статус "Упакован"
/// </summary>
internal class PackOrderProductCommandHandler(IUserIdentityProvider userIdentity, IOrderProductsRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PackOrderProductCommand>
{
    private readonly IUserIdentityProvider _userIdentity = userIdentity;
    private readonly IOrderProductsRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(PackOrderProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.OrderProductId, cancellationToken);

        if (_userIdentity.Id is null || product.SellerId != _userIdentity.Id)
            throw new AccessDeniedException();

        OrderProductStatus packedStatus = new(product, OrderProductStatusType.Packed);
        product.AddStatus(packedStatus);

        await _repository.UpdateAsync(product, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        // TODO: Send to AMQP
    }
}
