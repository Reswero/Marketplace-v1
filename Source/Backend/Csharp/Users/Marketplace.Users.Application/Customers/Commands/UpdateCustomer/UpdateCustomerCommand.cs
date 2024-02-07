using MediatR;

namespace Marketplace.Users.Application.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(int AccountId, string FirstName, string LastName) : IRequest;
