using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.GetProductFullInfo;

public record GetProductFullInfoQuery(int Id)
    : IRequest<ProductFullInfoVM>;
