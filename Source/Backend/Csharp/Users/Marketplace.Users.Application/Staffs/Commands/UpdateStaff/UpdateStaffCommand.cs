using MediatR;

namespace Marketplace.Users.Application.Staffs.Commands.UpdateStaff;

public record UpdateStaffCommand(int AccountId, string FirstName, string LastName) : IRequest;
