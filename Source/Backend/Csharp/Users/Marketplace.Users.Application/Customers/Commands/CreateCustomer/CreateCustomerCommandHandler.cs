using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Customers.Commands.CreateCustomer;

/// <summary>
/// Создание профиля покупателя
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class CreateCustomerCommandHandler(ICustomersRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCustomerCommand>
{
    private readonly ICustomersRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer = new(request.AccountId, request.FirstName, request.LastName);

        await _repository.AddAsync(customer, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
