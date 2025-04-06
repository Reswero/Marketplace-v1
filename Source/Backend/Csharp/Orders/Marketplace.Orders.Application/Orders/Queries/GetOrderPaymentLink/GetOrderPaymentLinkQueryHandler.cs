using Marketplace.Common.Identity.User;
using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Exceptions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Orders.ViewModels;
using Marketplace.Orders.Domain;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetOrderPaymentLink;

/// <summary>
/// Получить идентификатор платежа для заказа
/// </summary>
internal class GetOrderPaymentLinkQueryHandler(IOrdersRepository repository, IUnitOfWork unitOfWork,
    IUserIdentityProvider userIdentity, IPaymentServiceClient paymentClient)
    : IRequestHandler<GetOrderPaymentLinkQuery, PaymentVM>
{
    private readonly IOrdersRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserIdentityProvider _userIdentity = userIdentity;
    private readonly IPaymentServiceClient _paymentClient = paymentClient;

    public async Task<PaymentVM> Handle(GetOrderPaymentLinkQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetAsync(request.OrderId, cancellationToken);

        if (_userIdentity.Id is null || order.CustomerId != _userIdentity.Id)
            throw new AccessDeniedException();

        if (order.Statuses.Any(s => s.Type > OrderStatusType.Pending))
            throw new ImpossibleToPayOrderException();

        if (order.Statuses.Any(s => s.Type == OrderStatusType.Pending) is false)
        {
            OrderStatus pendingStatus = new(order, OrderStatusType.Pending);
            order.AddStatus(pendingStatus);

            await _repository.UpdateAsync(order, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        var paymentId = await _paymentClient.GetPaymentAsync(order.Id, cancellationToken);
        return new(paymentId);
    }
}
