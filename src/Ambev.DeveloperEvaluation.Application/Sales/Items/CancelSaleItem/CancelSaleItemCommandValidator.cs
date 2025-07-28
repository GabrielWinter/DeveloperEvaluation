using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelSaleItem;

public class CancelSaleItemCommandValidator : AbstractValidator<CancelSaleItemCommand>
{
    public CancelSaleItemCommandValidator()
    {
        RuleFor(sale => sale.SaleId).NotEmpty();
        RuleFor(sale => sale.ItemId).NotEmpty();
    }
}
