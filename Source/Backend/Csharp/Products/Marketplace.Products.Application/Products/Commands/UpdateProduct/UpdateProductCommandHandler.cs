﻿using FluentValidation;
using FluentValidation.Results;
using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.ViewModels;
using Marketplace.Products.Application.Products.ViewModels.Validators;
using Marketplace.Products.Domain.Categories;
using Marketplace.Products.Domain.Parameters;
using Marketplace.Products.Domain.Products;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.UpdateProduct;

internal class UpdateProductCommandHandler(IProductsRepository productsRepository,
    ICategoriesRepository categoriesRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductWithIdCommand>
{
    private readonly IProductsRepository _productsRepository = productsRepository;
    private readonly ICategoriesRepository _categoriesRepository = categoriesRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateProductWithIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _productsRepository.GetAsync(request.Id, cancellationToken);
        product.ChangeInfo(request.Name, request.Description, request.Price);

        var category = await _categoriesRepository.GetAsync(product.CategoryId, cancellationToken);
        var subcategory = category.Subсategories.FirstOrDefault(s => s.Id == request.SubcategoryId)
            ?? throw new ObjectNotFoundException(typeof(Subсategory), request.SubcategoryId);

        product.ChangeSubcategory(subcategory);

        var parameters = await GetParametersAsync(category, product, request.Parameters, cancellationToken);

        product.RemoveParameters([.. product.Parameters]);
        product.AddParameters(parameters);

        await _productsRepository.UpdateAsync(product, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private static async Task<ProductParameter[]> GetParametersAsync(Category category, Product product,
        List<UpdateProductParameterVM> requestParameters, CancellationToken cancellationToken)
    {
        var categoryParameters = category.Parameters.ToDictionary(p => p.Id);
        var categoryParametersIds = categoryParameters.Keys.ToHashSet();
        var parametersIds = product.Parameters.Select(p => p.Id).ToHashSet();

        var unexistingParameters = requestParameters.Where(p => p.Id == 0 &&
            categoryParametersIds.Contains(p.CategoryParameterId));
        var existingParameters = requestParameters.Where(p => parametersIds.Contains(p.Id) &&
            categoryParametersIds.Contains(p.CategoryParameterId));

        var parametersVMs = existingParameters.Union(unexistingParameters);

        List<ValidationFailure> validationErrors = [];
        foreach (var vm in parametersVMs)
        {
            var categoryParameter = categoryParameters[vm.CategoryParameterId];

            UpdateProductParameterVMValidator validator = new(categoryParameter);
            var result = await validator.ValidateAsync(vm, cancellationToken);

            validationErrors.AddRange(result.Errors);
        }

        if (validationErrors.Count > 0)
        {
            throw new ValidationException(validationErrors);
        }

        return parametersVMs.Select(p => new ProductParameter(p.Id, p.CategoryParameterId, product.Id, p.Value))
            .ToArray();
    }
}
