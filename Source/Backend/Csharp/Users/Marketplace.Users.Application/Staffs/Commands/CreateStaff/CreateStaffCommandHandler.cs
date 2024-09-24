using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Staffs.Commands.CreateStaff;

/// <summary>
/// Создание профиля персонала
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class CreateStaffCommandHandler(IStaffsRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateStaffCommand>
{
    private readonly IStaffsRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        Staff staff = new(request.AccountId, request.FirstName, request.LastName);

        await _repository.AddAsync(staff, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
