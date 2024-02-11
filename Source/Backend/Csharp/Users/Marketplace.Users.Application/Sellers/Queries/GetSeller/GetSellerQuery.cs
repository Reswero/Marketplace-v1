using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Sellers.Queries.GetSeller;

public record GetSellerQuery(int AccountId) : IRequest<Seller>;
