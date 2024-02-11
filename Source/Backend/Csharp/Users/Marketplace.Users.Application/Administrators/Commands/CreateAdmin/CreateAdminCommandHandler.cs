using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Administrators.Commands.CreateAdmin;

internal class CreateAdminCommandHandler(IAdministratorsRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAdminCommand>
{
    private readonly IAdministratorsRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
        Administrator admin = new(request.AccountId, request.FirstName, request.LastName);

        await _repository.AddAsync(admin, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
