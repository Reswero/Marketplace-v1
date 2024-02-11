using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Staffs.Commands.UpdateStaff;

internal class UpdateStaffCommandHandler(IStaffsRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateStaffCommand>
{
    private readonly IStaffsRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        Staff staff = new(request.AccountId, request.FirstName, request.LastName);

        await _repository.UpdateAsync(staff, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
