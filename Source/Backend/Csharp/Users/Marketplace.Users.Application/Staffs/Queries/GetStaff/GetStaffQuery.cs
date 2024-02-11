using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Staffs.Queries.GetStaff;

public record GetStaffQuery(int AccountId) : IRequest<Staff>;
