using MediatR;

namespace Marketplace.Users.Application.Administrators.Commands.CreateAdmin;

public record CreateAdminCommand(int AccountId, string FirstName, string LastName) : IRequest;
