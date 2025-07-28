using Ambev.DeveloperEvaluation.Application.Sales.Items.CancelSaleItem;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Items.CancelSaleItem;

public class CancelSaleItemCommandValidatorTests
{
    private readonly CancelSaleItemCommandValidator _validator = new();

    [Fact(DisplayName = "Comando válido deve passar")]
    public void Validate_ValidCommand_ShouldPass()
    {
        var command = new CancelSaleItemCommand(Guid.NewGuid(), Guid.NewGuid());

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Campos vazios devem falhar")]
    [InlineData("SaleId")]
    [InlineData("ItemId")]
    public void Validate_EmptyFields_ShouldFail(string field)
    {
        var saleId = field == "SaleId" ? Guid.Empty : Guid.NewGuid();
        var itemId = field == "ItemId" ? Guid.Empty : Guid.NewGuid();

        var command = new CancelSaleItemCommand(saleId, itemId);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == field);
    }
}
