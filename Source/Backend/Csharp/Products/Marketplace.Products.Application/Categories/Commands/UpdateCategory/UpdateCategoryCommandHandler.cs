﻿using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Domain.Parameters;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.UpdateCategory;

/// <summary>
/// Обновление категории
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class UpdateCategoryCommandHandler(ICategoriesRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ICategoriesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetAsync(request.Id, cancellationToken);
        category.SetName(request.Name);

        var parametersIds = category.Parameters.Select(p => p.Id).ToList();

        var unexistingParameters = request.Parameters.Where(p => p.Id == 0).ToList();
        var existingParameters = request.Parameters.Where(p => parametersIds.Contains(p.Id)).ToList();

        var parametersToAdd = existingParameters.Select(p =>
            new CategoryParameter(category.Id, p.Id, p.Name, p.Type)).ToArray();

        category.RemoveParameters([.. category.Parameters]);
        category.AddParameters(parametersToAdd);

        await _repository.UpdateAsync(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}