using Ambev.DeveloperEvaluation.Application.Sales.Items.AddSaleItem;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Items.AddSaleItem;

public class AddSaleItemCommandValidatorTests
{
    private readonly AddSaleItemCommandValidator _validator = new();

    private AddSaleItemCommand CreateValidCommand() => new()
    {
        SaleId = Guid.NewGuid(),
        ProductId = Guid.NewGuid(),
        ProductName = "Produto Teste",
        Quantity = 10,
        UnitPrice = 99.99m
    };

    [Fact(DisplayName = "Command válido deve passar")]
    public void Validate_ValidCommand_ShouldBeValid()
    {
        var command = CreateValidCommand();

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Propriedades inválidas devem falhar individualmente")]
    [InlineData("SaleId")]
    [InlineData("ProductId")]
    [InlineData("ProductName")]
    [InlineData("Quantity")]
    [InlineData("UnitPrice")]
    public void Validate_InvalidFields_ShouldFailValidation(string propertyName)
    {
        var command = CreateValidCommand();

        switch (propertyName)
        {
            case "SaleId": command.SaleId = Guid.Empty; break;
            case "ProductId": command.ProductId = Guid.Empty; break;
            case "ProductName": command.ProductName = ""; break;
            case "Quantity": command.Quantity = 0; break;
            case "UnitPrice": command.UnitPrice = -1; break;
        }

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == propertyName);
    }
}