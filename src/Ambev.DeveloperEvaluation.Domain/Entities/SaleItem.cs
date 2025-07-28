using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; set; }
    public Sale? Sale { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal UnitDiscount { get; set; }
    public decimal TotalDiscount => Quantity * UnitDiscount;
    public decimal Subtotal => Quantity * UnitPrice;
    public decimal Total => Subtotal - TotalDiscount;
    public bool IsCancelled { get; set; }

    public void ApplyDiscount(decimal discountRate)
    {
        UnitDiscount = discountRate * UnitPrice;
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}
