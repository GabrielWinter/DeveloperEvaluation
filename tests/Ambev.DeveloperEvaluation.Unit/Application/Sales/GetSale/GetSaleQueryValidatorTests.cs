using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

public class GetSaleQueryValidatorTests
{
    private readonly GetSaleQueryValidator _validator = new();

    [Fact(DisplayName = "Valid ID should pass validation")]
    public void Validate_ValidId_ShouldBeValid()
    {
        // Arrange
        var query = new GetSaleQuery(Guid.NewGuid());

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Empty ID should fail validation")]
    public void Validate_EmptyId_ShouldBeInvalid()
    {
        // Arrange
        var query = new GetSaleQuery(Guid.Empty);

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetSaleQuery.Id));
    }
}