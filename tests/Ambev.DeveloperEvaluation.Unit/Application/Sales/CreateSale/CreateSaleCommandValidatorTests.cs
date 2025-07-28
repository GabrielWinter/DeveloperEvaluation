using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator = new();

    [Fact(DisplayName = "Given a valid command, validator should succeed")]
    public void Validate_ValidCommand_ShouldBeValid()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch 001",
            Number = 123,
            Date = DateTime.UtcNow,
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    Quantity = 2,
                    UnitPrice = 10.5m
                }
            }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Given empty fields, validator should return errors")]
    public void Validate_InvalidCommand_MissingFields_ShouldHaveErrors()
    {
        // Arrange
        var command = new CreateSaleCommand();

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "CustomerId");
        result.Errors.Should().Contain(x => x.PropertyName == "CustomerName");
        result.Errors.Should().Contain(x => x.PropertyName == "BranchId");
        result.Errors.Should().Contain(x => x.PropertyName == "BranchName");
        result.Errors.Should().Contain(x => x.PropertyName == "Number");
    }

    [Fact(DisplayName = "Given item with invalid data, validator should return item errors")]
    public void Validate_InvalidItem_ShouldHaveItemErrors()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "Valid Customer",
            BranchId = Guid.NewGuid(),
            BranchName = "Valid Branch",
            Number = 100,
            Date = DateTime.UtcNow,
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = Guid.Empty,
                    ProductName = "",
                    Quantity = 0,
                    UnitPrice = -1
                }
            }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName.Contains("Items[0].ProductId"));
        result.Errors.Should().Contain(x => x.PropertyName.Contains("Items[0].ProductName"));
        result.Errors.Should().Contain(x => x.PropertyName.Contains("Items[0].Quantity"));
        result.Errors.Should().Contain(x => x.PropertyName.Contains("Items[0].UnitPrice"));
    }
}