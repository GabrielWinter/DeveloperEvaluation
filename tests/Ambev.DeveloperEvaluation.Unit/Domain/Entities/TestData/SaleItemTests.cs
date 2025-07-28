using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public class SaleItemTests
{
    [Fact(DisplayName = "TotalDiscount should calculate correctly based on quantity and unit discount")]
    public void TotalDiscount_ShouldCalculateCorrectly()
    {
        var item = new SaleItem
        {
            Quantity = 3,
            UnitDiscount = 5m
        };

        item.TotalDiscount.Should().Be(15m);
    }

    [Fact(DisplayName = "Subtotal should calculate correctly based on quantity and unit price")]
    public void Subtotal_ShouldCalculateCorrectly()
    {
        var item = new SaleItem
        {
            Quantity = 4,
            UnitPrice = 10m
        };

        item.Subtotal.Should().Be(40m);
    }

    [Fact(DisplayName = "Total should be equal to Subtotal minus TotalDiscount")]
    public void Total_ShouldBeSubtotalMinusTotalDiscount()
    {
        var item = new SaleItem
        {
            Quantity = 5,
            UnitPrice = 20m,
            UnitDiscount = 2m
        };

        item.Subtotal.Should().Be(100m);
        item.TotalDiscount.Should().Be(10m);
        item.Total.Should().Be(90m);
    }

    [Fact(DisplayName = "Cancel should mark the item as cancelled")]
    public void Cancel_ShouldMarkItemAsCancelled()
    {
        var item = new SaleItem();

        item.Cancel();

        item.IsCancelled.Should().BeTrue();
    }

    [Fact(DisplayName = "ApplyDiscount should set UnitDiscount correctly based on rate")]
    public void ApplyDiscount_ShouldSetUnitDiscountCorrectly()
    {
        var item = new SaleItem
        {
            UnitPrice = 100m
        };

        item.ApplyDiscount(0.2m);

        item.UnitDiscount.Should().Be(20m);
    }
}