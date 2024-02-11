using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Administrators.Queries;

public record GetAdminQuery(int AccountId) : IRequest<Administrator>;
