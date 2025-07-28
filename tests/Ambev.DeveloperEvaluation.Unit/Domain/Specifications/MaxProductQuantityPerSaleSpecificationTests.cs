using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications;

public class MaxProductQuantityPerSaleSpecificationTests
{
    private readonly Guid _productId = Guid.NewGuid();

    [Fact(DisplayName = "IsSatisfiedBy should return true when total quantity is less than or equal to 20")]
    public void IsSatisfiedBy_ShouldReturnTrue_WhenQuantityIsWithinLimit()
    {
        // Arrange
        var sale = new Sale();
        sale.AddItem(new SaleItem
        {
            ProductId = _productId,
            Quantity = 10,
            UnitPrice = 10m
        });

        var specification = new MaxProductQuantityPerSaleSpecification(_productId, 10);

        // Act
        var result = specification.IsSatisfiedBy(sale);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "IsSatisfiedBy should return false when total quantity exceeds 20")]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenQuantityExceedsLimit()
    {
        // Arrange
        var sale = new Sale();
        sale.AddItem(new SaleItem
        {
            ProductId = _productId,
            Quantity = 15,
            UnitPrice = 10m
        });

        var specification = new MaxProductQuantityPerSaleSpecification(_productId, 6);

        // Act
        var result = specification.IsSatisfiedBy(sale);

        // Assert
        result.Should().BeFalse();
    }

    [Fact(DisplayName = "IsSatisfiedBy should consider only matching product ID")]
    public void IsSatisfiedBy_ShouldIgnoreDifferentProductIds()
    {
        // Arrange
        var sale = new Sale();
        sale.AddItem(new SaleItem
        {
            ProductId = Guid.NewGuid(),
            Quantity = 20,
            UnitPrice = 10m
        });

        var specification = new MaxProductQuantityPerSaleSpecification(_productId, 5);

        // Act
        var result = specification.IsSatisfiedBy(sale);

        // Assert
        result.Should().BeTrue();
    }
}
