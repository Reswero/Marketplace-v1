using MediatR;

namespace Marketplace.Users.Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(int AccountId, string FirstName, string LastName) : IRequest;
