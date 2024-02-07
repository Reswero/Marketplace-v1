using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Customers.Queries.GetCustomer;

public record GetCustomerQuery(int AccountId) : IRequest<Customer>;
