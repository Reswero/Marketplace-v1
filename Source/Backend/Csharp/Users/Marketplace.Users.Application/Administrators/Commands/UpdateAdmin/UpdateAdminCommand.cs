using MediatR;

namespace Marketplace.Users.Application.Administrators.Commands.UpdateAdmin;

public record UpdateAdminCommand(int AccountId, string FirstName, string LastName) : IRequest;
