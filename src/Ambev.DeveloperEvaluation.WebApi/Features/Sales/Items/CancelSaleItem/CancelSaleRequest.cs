namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelSaleItem;

public class CancelSaleItemRequest
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
}
