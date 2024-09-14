using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
    public sealed class CreateProductRequestValidator:AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x=>x.Name).NotNull().WithMessage("ProductName is required").Length(3,10).WithMessage("Name have to be 3-10 character");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price have to be bigger than zero.");

            RuleFor(x => x.Stock).InclusiveBetween(1, 100).WithMessage("Stock quantity have to be 100 between 1 ");

        }
    }
}
