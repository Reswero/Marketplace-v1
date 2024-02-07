using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Customers.Commands.UpdateCustomer;

internal class UpdateCustomerCommandHandler(ICustomersRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCustomerCommand>
{
    private readonly ICustomersRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer = new(request.AccountId, request.FirstName, request.LastName);

        await _repository.UpdateAsync(customer, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
