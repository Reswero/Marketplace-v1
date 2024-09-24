using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Sellers.Commands.UpdateSeller;

/// <summary>
/// Обновление профиля продавца
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class UpdateSellerCommandHandler(ISellersRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSellerCommand>
{
    private readonly ISellersRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateSellerCommand request, CancellationToken cancellationToken)
    {
        Seller seller = new(request.AccountId, request.FirstName, request.LastName, request.CompanyName);

        await _repository.UpdateAsync(seller, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
