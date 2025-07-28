namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleItemResult
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal UnitDiscount { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public bool IsCancelled { get; set; }
}
