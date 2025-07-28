using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

public class CreateSaleItemCommandValidatorTests
{
    private readonly CreateSaleItemCommandValidator _validator = new();

    [Fact(DisplayName = "Given a valid item, validator should succeed")]
    public void Validate_ValidItem_ShouldBeValid()
    {
        // Arrange
        var item = new CreateSaleItemCommand
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Product A",
            Quantity = 5,
            UnitPrice = 9.99m
        };

        // Act
        var result = _validator.Validate(item);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Given empty ProductId, validator should fail")]
    public void Validate_EmptyProductId_ShouldFail()
    {
        var item = new CreateSaleItemCommand
        {
            ProductId = Guid.Empty,
            ProductName = "Product A",
            Quantity = 5,
            UnitPrice = 9.99m
        };

        var result = _validator.Validate(item);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ProductId");
    }

    [Fact(DisplayName = "Given empty ProductName, validator should fail")]
    public void Validate_EmptyProductName_ShouldFail()
    {
        var item = new CreateSaleItemCommand
        {
            ProductId = Guid.NewGuid(),
            ProductName = "",
            Quantity = 5,
            UnitPrice = 9.99m
        };

        var result = _validator.Validate(item);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ProductName");
    }

    [Theory(DisplayName = "Given invalid Quantity, validator should fail")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidQuantity_ShouldFail(int invalidQuantity)
    {
        var item = new CreateSaleItemCommand
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Product A",
            Quantity = invalidQuantity,
            UnitPrice = 9.99m
        };

        var result = _validator.Validate(item);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Quantity");
    }

    [Fact(DisplayName = "Given negative UnitPrice, validator should fail")]
    public void Validate_NegativeUnitPrice_ShouldFail()
    {
        var item = new CreateSaleItemCommand
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Product A",
            Quantity = 5,
            UnitPrice = -10m
        };

        var result = _validator.Validate(item);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UnitPrice");
    }
}