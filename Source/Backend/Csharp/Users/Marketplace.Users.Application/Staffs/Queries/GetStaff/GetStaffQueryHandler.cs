using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Staffs.Queries.GetStaff;

internal class GetStaffQueryHandler(IStaffsRepository repository) : IRequestHandler<GetStaffQuery, Staff>
{
    private readonly IStaffsRepository _repository = repository;
    public async Task<Staff> Handle(GetStaffQuery request, CancellationToken cancellationToken)
    {
        var staff = await _repository.GetAsync(request.AccountId, cancellationToken);
        return staff;
    }
}
