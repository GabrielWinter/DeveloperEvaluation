using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelSaleItem;

public class CancelSaleItemRequestValidator : AbstractValidator<CancelSaleItemRequest>
{
    public CancelSaleItemRequestValidator()
    {
        RuleFor(sale => sale.SaleId).NotEmpty();
        RuleFor(sale => sale.ItemId).NotEmpty();
    }
}
