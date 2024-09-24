using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Customers.Queries.GetCustomer;

/// <summary>
/// Получение профиля покупателя
/// </summary>
/// <param name="repository"></param>
internal class GetCustomerQueryHandler(ICustomersRepository repository) : IRequestHandler<GetCustomerQuery, Customer>
{
    private readonly ICustomersRepository _repository = repository;

    public async Task<Customer> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetAsync(request.AccountId, cancellationToken);
        return customer;
    }
}
