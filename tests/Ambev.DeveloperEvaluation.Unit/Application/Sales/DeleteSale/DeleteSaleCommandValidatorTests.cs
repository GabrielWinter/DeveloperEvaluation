using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.DeleteSale;

public class DeleteSaleCommandValidatorTests
{
    private readonly DeleteSaleCommandValidator _validator = new();

    [Fact(DisplayName = "Given valid Id, validator should succeed")]
    public void Validate_ValidId_ShouldBeValid()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Given empty Id, validator should fail")]
    public void Validate_EmptyId_ShouldFail()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.Empty);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }
}

