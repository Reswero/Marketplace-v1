using MediatR;

namespace Marketplace.Users.Application.Sellers.Commands.CreateSeller;

public record CreateSellerCommand(int AccountId, string FirstName, string LastName,
    string CompanyName) : IRequest;
