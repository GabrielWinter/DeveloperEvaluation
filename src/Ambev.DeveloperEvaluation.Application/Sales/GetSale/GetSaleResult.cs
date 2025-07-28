namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleResult
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
    public IEnumerable<GetSaleItemResult> Items { get; set; } = Array.Empty<GetSaleItemResult>();
}
