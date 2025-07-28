using Ambev.DeveloperEvaluation.Domain.Policies;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Policies;

public class QuantityBasedDiscountPolicyTests
{
    private readonly QuantityBasedDiscountPolicy _policy = new();

    [Theory(DisplayName = "GetDiscountRate should return 0% for quantities less than 4")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetDiscountRate_ShouldReturnZero_ForQuantitiesLessThanFour(int quantity)
    {
        var result = _policy.GetDiscountRate(quantity);
        result.Should().Be(0m);
    }

    [Theory(DisplayName = "GetDiscountRate should return 10% for quantities between 4 and 9")]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(9)]
    public void GetDiscountRate_ShouldReturnTenPercent_ForQuantitiesBetweenFourAndNine(int quantity)
    {
        var result = _policy.GetDiscountRate(quantity);
        result.Should().Be(0.10m);
    }

    [Theory(DisplayName = "GetDiscountRate should return 20% for quantities between 10 and 20")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void GetDiscountRate_ShouldReturnTwentyPercent_ForQuantitiesBetweenTenAndTwenty(int quantity)
    {
        var result = _policy.GetDiscountRate(quantity);
        result.Should().Be(0.20m);
    }

    [Theory(DisplayName = "GetDiscountRate should return 0% for quantities greater than 20")]
    [InlineData(21)]
    [InlineData(100)]
    public void GetDiscountRate_ShouldReturnZero_ForQuantitiesGreaterThanTwenty(int quantity)
    {
        var result = _policy.GetDiscountRate(quantity);
        result.Should().Be(0m);
    }
}