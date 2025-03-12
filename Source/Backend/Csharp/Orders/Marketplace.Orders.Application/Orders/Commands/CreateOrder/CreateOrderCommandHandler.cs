using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Exceptions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Common.ViewModels;
using Marketplace.Orders.Application.Integrations.Products;
using Marketplace.Orders.Domain;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Commands.CreateOrder;

/// <summary>
/// Создать заказ
/// </summary>
internal class CreateOrderCommandHandler(IOrdersRepository ordersRepository, INewOrdersRepository newOrdersRepository,
    IUnitOfWork unitOfWork, IProductsServiceClient productsClient)
    : IRequestHandler<CreateOrderCommand, CreateObjectResultVM>
{
    private readonly IOrdersRepository _ordersRepository = ordersRepository;
    private readonly INewOrdersRepository _newOrdersRepository = newOrdersRepository;
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

            if (product.Status != ProductStatus.Available)
                throw new ProductNotAvailableException(product.Id);

            OrderProduct orderProduct = new(order, product.Id, product.SellerId,
                requestProduct.Quantity, product.Price, product.DiscountSize);
            orderProducts[i] = orderProduct;

            OrderProductStatus createdProductStatus = new(orderProduct, OrderProductStatusType.Packing);
            orderProduct.AddStatus(createdProductStatus);
        }

        order.AddProducts(orderProducts);

        OrderStatus createdStatus = new(order, OrderStatusType.Created);
        order.AddStatus(createdStatus);

        NewOrder newOrder = new(order);

        await _ordersRepository.AddAsync(order, cancellationToken);
        await _newOrdersRepository.AddAsync(newOrder, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new CreateObjectResultVM(order.Id);
    }
}
