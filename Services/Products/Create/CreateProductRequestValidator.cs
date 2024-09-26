using App.Repositories.Products;
using FluentValidation;

namespace App.Services.Products.Create
{
    public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IProductRepository _productRepository;
        public CreateProductRequestValidator(IProductRepository productRepository)
        {
            RuleFor(x => x.Name).NotNull().WithMessage("ProductName is required").Length(3, 10).WithMessage("Name have to be 3-10 character");
            //.MustAsync(MustUniqueProductNameAsync).WithMessage("Name already exists");
            //.Must(MustUniqueProductName).WithMessage("Product name  already exists");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price have to be bigger than zero.");

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category have to be bigger than zero.");

            RuleFor(x => x.Stock).InclusiveBetween(1, 100).WithMessage("Stock quantity have to be 100 between 1 ");
            _productRepository = productRepository;
        }

        // first method with sync validation
        //private bool MustUniqueProductName(string productName)
        //{
        //    return !_productRepository.Where(x=>x.Name == productName).Any();

        //}

        //private async Task<bool> MustUniqueProductNameAsync(string productName ,CancellationToken cancellationToken)
        //{
        //    return !await _productRepository.Where(x => x.Name == productName).AnyAsync(cancellationToken);
        //}

    }
}
