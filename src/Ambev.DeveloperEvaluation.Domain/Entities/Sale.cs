using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = null!;
    public int Number { get; set; }
    public DateTime Date { get; set; }
    public decimal Discount { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public List<SaleItem> Items { get; set; } = new();
    public bool IsCancelled { get; set; }

    public void AddItem(SaleItem item)
    {
        var maxQuantitySpec = new MaxProductQuantityPerSaleSpecification(item.ProductId, item.Quantity);

        if (!maxQuantitySpec.IsSatisfiedBy(this))
            throw new DomainException("Exceed a maximum of 20 units per product");

        Items.Add(item);

        ApplyItemTotals(item);
    }

    public void AddItemRange(IEnumerable<SaleItem> items)
    {
        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    public void Cancel()
    {
        if (IsCancelled)
            return;

        IsCancelled = true;

        foreach (var item in Items)
        {
            item.Cancel();
        }
    }

    public SaleItem CancelItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null || item.IsCancelled)
            return null;

        item.Cancel();
        RevertItemTotals(item);

        return item;
    }

    public void RecalculateTotals()
    {
        ResetTotals();

        foreach (var item in Items.Where(i => !i.IsCancelled))
        {
            ApplyItemTotals(item);
        }
    }

    public void Update(
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName,
        int number,
        DateTime date)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
        Number = number;
        Date = date;
    }

    private void ResetTotals()
    {
        Discount = 0m;
        Subtotal = 0m;
        Total = 0m;
    }

    private void ApplyItemTotals(SaleItem item)
    {
        Discount += item.TotalDiscount;
        Subtotal += item.Subtotal;
        Total += item.Total;
    }

    private void RevertItemTotals(SaleItem item)
    {
        Discount -= item.TotalDiscount;
        Subtotal -= item.Subtotal;
        Total -= item.Total;
    }

}
