using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "AddItem should update totals correctly when item is valid")]
    public void AddItem_ShouldUpdateTotalsCorrectly_WhenItemIsValid()
    {
        var sale = new Sale();
        var item = new SaleItem
        {
            Id = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Product A",
            Quantity = 2,
            UnitPrice = 100,
            UnitDiscount = 10
        };

        sale.AddItem(item);

        sale.Items.Should().ContainSingle(i => i.Id == item.Id);
        sale.Discount.Should().Be(20);
        sale.Subtotal.Should().Be(200);
        sale.Total.Should().Be(180);
    }

    [Fact(DisplayName = "AddItem should throw exception when quantity exceeds limit")]
    public void AddItem_ShouldThrowException_WhenQuantityExceedsLimit()
    {
        var sale = new Sale();
        var item = new SaleItem
        {
            Id = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Product A",
            Quantity = 21,
            UnitPrice = 50
        };

        var act = () => sale.AddItem(item);

        act.Should().Throw<DomainException>()
            .WithMessage("Exceed a maximum of 20 units per product");
    }

    [Fact(DisplayName = "Cancel should set IsCancelled to true and cancel all items")]
    public void Cancel_ShouldSetIsCancelledToTrue_AndCancelAllItems()
    {
        var sale = new Sale();
        var item1 = new SaleItem { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), ProductName = "P1", Quantity = 1, UnitPrice = 10 };
        var item2 = new SaleItem { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), ProductName = "P2", Quantity = 2, UnitPrice = 15 };
        sale.AddItem(item1);
        sale.AddItem(item2);

        sale.Cancel();

        sale.IsCancelled.Should().BeTrue();
        sale.Items.Should().OnlyContain(i => i.IsCancelled);
    }

    [Fact(DisplayName = "CancelItem should cancel the correct item and update totals")]
    public void CancelItem_ShouldCancelCorrectItem_AndUpdateTotals()
    {
        var sale = new Sale();
        var item = new SaleItem
        {
            Id = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Product B",
            Quantity = 3,
            UnitPrice = 50,
            UnitDiscount = 5
        };
        sale.AddItem(item);

        var cancelled = sale.CancelItem(item.Id);

        cancelled.Should().NotBeNull();
        cancelled!.IsCancelled.Should().BeTrue();
        sale.Discount.Should().Be(0);
        sale.Subtotal.Should().Be(0);
        sale.Total.Should().Be(0);
    }

    [Fact(DisplayName = "CancelItem should return null when item is not found")]
    public void CancelItem_ShouldReturnNull_WhenItemNotFound()
    {
        var sale = new Sale();

        var result = sale.CancelItem(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact(DisplayName = "RecalculateTotals should sum only non-cancelled items")]
    public void RecalculateTotals_ShouldSumAllNonCancelledItems()
    {
        var sale = new Sale();
        var item1 = new SaleItem { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), ProductName = "A", Quantity = 2, UnitPrice = 20, UnitDiscount = 5 };
        var item2 = new SaleItem { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), ProductName = "B", Quantity = 1, UnitPrice = 100, UnitDiscount = 10 };
        var item3 = new SaleItem { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), ProductName = "C", Quantity = 5, UnitPrice = 10, UnitDiscount = 2 };

        sale.AddItem(item1);
        sale.AddItem(item2);
        sale.AddItem(item3);

        item2.Cancel();

        sale.RecalculateTotals();

        sale.Discount.Should().Be(item1.TotalDiscount + item3.TotalDiscount);
        sale.Subtotal.Should().Be(item1.Subtotal + item3.Subtotal);
        sale.Total.Should().Be(item1.Total + item3.Total);
    }

    [Fact(DisplayName = "Update should replace all customer, branch, and sale data")]
    public void Update_ShouldReplaceAllCustomerBranchAndSaleData()
    {
        var sale = new Sale();
        var newCustomerId = Guid.NewGuid();
        var newBranchId = Guid.NewGuid();
        var date = DateTime.Today;

        sale.Update(newCustomerId, "Customer Test", newBranchId, "Branch Test", 99, date);

        sale.CustomerId.Should().Be(newCustomerId);
        sale.CustomerName.Should().Be("Customer Test");
        sale.BranchId.Should().Be(newBranchId);
        sale.BranchName.Should().Be("Branch Test");
        sale.Number.Should().Be(99);
        sale.Date.Should().Be(date);
    }
}