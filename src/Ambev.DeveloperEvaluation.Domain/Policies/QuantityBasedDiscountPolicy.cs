namespace Ambev.DeveloperEvaluation.Domain.Policies;

public class QuantityBasedDiscountPolicy
{
    public decimal GetDiscountRate(int quantity)
    {
        if (quantity >= 4 && quantity < 10)
            return 0.10m;

        if (quantity >= 10 && quantity <= 20)
            return 0.20m;

        return 0m;
    }
}
