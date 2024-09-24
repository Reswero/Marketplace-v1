using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Administrators.Queries.GetAdmin;

/// <summary>
/// Получение профиля администратора
/// </summary>
/// <param name="repository"></param>
internal class GetAdminQueryHandler(IAdministratorsRepository repository) : IRequestHandler<GetAdminQuery, Administrator>
{
    private readonly IAdministratorsRepository _repository = repository;

    public async Task<Administrator> Handle(GetAdminQuery request, CancellationToken cancellationToken)
    {
        var admin = await _repository.GetAsync(request.AccountId, cancellationToken);
        return admin;
    }
}
