using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Exceptions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Common.ViewModels;
using Marketplace.Orders.Domain;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Commands.CreateOrder;

internal class CreateOrderCommandHandler(IOrdersRepository repository, IUnitOfWork unitOfWork,
    IProductsServiceClient productsClient)
    : IRequestHandler<CreateOrderCommand, CreateObjectResultVM>
{
    private readonly IOrdersRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IProductsServiceClient _productsClient = productsClient;

    public async Task<CreateObjectResultVM> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = new(request.CustomerId);

        var requestProducts = request.Products.ToDictionary(p => p.Id);
        var products = await _productsClient.GetProductsByIdsAsync([.. requestProducts.Keys], cancellationToken);
        var productsDictionary = products.ToDictionary(p => p.Id);

        foreach (var requestProduct in requestProducts)
        {
            if (productsDictionary.ContainsKey(requestProduct.Key) is false)
                throw new ProductNotFoundException(requestProduct.Key);
        }

        var orderProducts = new OrderProduct[products.Count];
        for (int i = 0; i < products.Count; i++)
        {
            var product = products[i];
            var requestProduct = requestProducts[product.Id];

            OrderProduct orderProduct = new(order, product.Id, product.SellerId,
                requestProduct.Quantity, product.Price, product.DiscountSize);
            orderProducts[i] = orderProduct;
        }

        order.AddProducts(orderProducts);

        OrderStatus createdStatus = new(order, OrderStatusType.Created);
        order.AddStatus(createdStatus);

        await _repository.AddAsync(order, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        // TODO: call to payment service
        OrderStatus pendingStatus = new(order, OrderStatusType.Pending);
        order.AddStatus(pendingStatus);

        await _repository.UpdateAsync(order, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        // TODO: return link to payment
        return new CreateObjectResultVM(order.Id);
    }
}
