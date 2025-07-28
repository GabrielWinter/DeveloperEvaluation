using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale;

public class UpdateSaleCommandValidatorTests
{
    private readonly UpdateSaleCommandValidator _validator = new();

    private UpdateSaleCommand CreateValidCommand() => new()
    {
        Id = Guid.NewGuid(),
        CustomerId = Guid.NewGuid(),
        CustomerName = "Valid Customer",
        BranchId = Guid.NewGuid(),
        BranchName = "Branch A",
        Number = 123,
        Date = DateTime.UtcNow
    };

    [Fact(DisplayName = "Valid command should pass validation")]
    public void Validate_ValidCommand_ShouldBeValid()
    {
        var command = CreateValidCommand();

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Invalid command should fail on individual properties")]
    [InlineData("Id")]
    [InlineData("CustomerId")]
    [InlineData("CustomerName")]
    [InlineData("BranchId")]
    [InlineData("BranchName")]
    [InlineData("Number")]
    [InlineData("Date")]
    public void Validate_InvalidCommand_ShouldFail(string propertyName)
    {
        var command = CreateValidCommand();

        switch (propertyName)
        {
            case "Id": command.Id = Guid.Empty; break;
            case "CustomerId": command.CustomerId = Guid.Empty; break;
            case "CustomerName": command.CustomerName = ""; break;
            case "BranchId": command.BranchId = Guid.Empty; break;
            case "BranchName": command.BranchName = ""; break;
            case "Number": command.Number = 0; break;
            case "Date": command.Date = DateTime.MinValue; break;
        }

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == propertyName);
    }
}
