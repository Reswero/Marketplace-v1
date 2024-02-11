using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Sellers.Commands.CreateSeller;

internal class CreateSellerCommandHandler(ISellersRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSellerCommand>
{
    private readonly ISellersRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateSellerCommand request, CancellationToken cancellationToken)
    {
        Seller seller = new(request.AccountId, request.FirstName, request.LastName, request.CompanyName);

        await _repository.AddAsync(seller, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
