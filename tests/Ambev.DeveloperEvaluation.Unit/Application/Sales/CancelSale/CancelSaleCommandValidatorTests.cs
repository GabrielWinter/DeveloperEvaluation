using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CancelSale;

public class CancelSaleCommandValidatorTests
{
    private readonly CancelSaleCommandValidator _validator = new();

    [Fact(DisplayName = "Given valid command should pass validation")]
    public void Validate_ValidCommand_ShouldPass()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }

    [Fact(DisplayName = "Given empty Id should fail validation")]
    public void Validate_EmptyId_ShouldFail()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
}
