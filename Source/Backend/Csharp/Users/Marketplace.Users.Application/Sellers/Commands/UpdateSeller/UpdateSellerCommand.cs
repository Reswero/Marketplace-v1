using MediatR;

namespace Marketplace.Users.Application.Sellers.Commands.UpdateSeller;

public record UpdateSellerCommand(int AccountId, string FirstName, string LastName,
    string CompanyName) : IRequest;
