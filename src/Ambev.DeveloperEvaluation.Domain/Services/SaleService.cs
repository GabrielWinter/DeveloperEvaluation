using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Policies;
namespace Ambev.DeveloperEvaluation.Domain.Services;

public class SaleService : ISaleService
{
    public void CalculateAndApplyItemDiscounts(Sale sale)
    {
        var discountPolicy = new QuantityBasedDiscountPolicy();
        var itemsGroupedByProduct = sale.Items.GroupBy(item => item.ProductId);

        var productDiscounts = itemsGroupedByProduct
            .ToDictionary(
                group => group.Key,
                group => discountPolicy.GetDiscountRate(group.Sum(item => item.Quantity))
            );

        foreach (var item in sale.Items)
        {
            if (productDiscounts.TryGetValue(item.ProductId, out decimal discountRate))
            {
                item.ApplyDiscount(discountRate);
            }
        }

        sale.RecalculateTotals();
    }
}
