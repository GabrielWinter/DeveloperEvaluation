using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddSaleItem;

public class AddSaleItemCommand : IRequest<AddSaleItemResult>
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
