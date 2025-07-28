using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddSaleItem;

public class AddSaleItemCommandValidator : AbstractValidator<AddSaleItemCommand>
{
    public AddSaleItemCommandValidator()
    {
        RuleFor(item => item.SaleId).NotEmpty();
        RuleFor(item => item.ProductId).NotEmpty();
        RuleFor(item => item.ProductName).NotEmpty();
        RuleFor(item => item.Quantity).GreaterThan(0);
        RuleFor(item => item.UnitPrice).GreaterThanOrEqualTo(0);
    }
}
