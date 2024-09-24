using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Administrators.Commands.UpdateAdmin;

/// <summary>
/// Обновление профиля администратора
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class UpdateAdminCommandHandler(IAdministratorsRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateAdminCommand>
{
    private readonly IAdministratorsRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateAdminCommand request, CancellationToken cancellationToken)
    {
        Administrator admin = new(request.AccountId, request.FirstName, request.LastName);

        await _repository.UpdateAsync(admin, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
