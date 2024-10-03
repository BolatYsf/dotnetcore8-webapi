using FluentValidation;

namespace App.Application.Features.Products.Update
{
    public class UpdateProductRequestValidator:AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(1, 10).WithMessage("Name have to be 3-10 character");

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category have to be bigger than zero.");

        }
    }
}
