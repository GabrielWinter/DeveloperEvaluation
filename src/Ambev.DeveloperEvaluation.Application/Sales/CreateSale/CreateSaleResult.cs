namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleResult
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = null!;
    public int Number { get; set; }
    public DateTime Date { get; set; }
    public decimal Discount { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public bool IsCancelled { get; set; }
    public IEnumerable<CreateSaleItemResult> Items { get; set; } = Array.Empty<CreateSaleItemResult>();
}
