using MediatR;

namespace Marketplace.Users.Application.Staffs.Commands.CreateStaff;

public record CreateStaffCommand(int AccountId, string FirstName, string LastName) : IRequest;
