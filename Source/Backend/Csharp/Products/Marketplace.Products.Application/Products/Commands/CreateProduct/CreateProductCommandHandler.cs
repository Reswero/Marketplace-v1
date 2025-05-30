﻿using FluentValidation;
using FluentValidation.Results;
using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Common.ViewModels;
using Marketplace.Products.Application.Products.ViewModels;
using Marketplace.Products.Application.Products.ViewModels.Validators;
using Marketplace.Products.Domain.Categories;
using Marketplace.Products.Domain.Parameters;
using Marketplace.Products.Domain.Products;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.CreateProduct;

/// <summary>
/// Создание товара
/// </summary>
/// <param name="productsRepository"></param>
/// <param name="categoriesRepository"></param>
/// <param name="unitOfWork"></param>
internal class CreateProductCommandHandler(IProductsRepository productsRepository,
    ICategoriesRepository categoriesRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductWithSellerIdCommand, CreateObjectResultVM>
{
    private readonly IProductsRepository _productsRepository = productsRepository;
    private readonly ICategoriesRepository _categoriesRepository = categoriesRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateObjectResultVM> Handle(CreateProductWithSellerIdCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetAsync(request.CategoryId, cancellationToken);
        var subcategory = category.Subсategories.FirstOrDefault(s => s.Id == request.SubcategoryId)
            ?? throw new ObjectNotFoundException(typeof(Subсategory), request.SubcategoryId);

        Product product = new(request.SellerId, category, subcategory,
            request.Name, request.Description, request.Price);

        var parameters = await GetParametersAsync(category, product, request.Parameters, cancellationToken);
        product.AddParameters(parameters);

        await _productsRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new(product.Id);
    }

    public static async Task<ProductParameter[]> GetParametersAsync(Category category, Product product,
        List<AddProductParameterVM> requestParameters, CancellationToken cancellationToken)
    {
        List<ValidationFailure> validationErrors = [];
        var parameters = new ProductParameter[requestParameters.Count];
        for (int i = 0; i < requestParameters.Count; i++)
        {
            var vm = requestParameters[i];

            var categoryParameter = category.Parameters.FirstOrDefault(p => p.Id == vm.CategoryParameterId)
                ?? throw new ObjectNotFoundException(typeof(CategoryParameter), vm.CategoryParameterId);

            AddProductParameterVMValidator validator = new(categoryParameter);
            var result = await validator.ValidateAsync(vm, cancellationToken);
            if (result.IsValid == false)
            {
                validationErrors.AddRange(result.Errors);
            }

            parameters[i] = new(categoryParameter, product, vm.Value);
        }

        if (validationErrors.Count > 0)
        {
            throw new ValidationException(validationErrors);
        }

        return parameters;
    }
}
