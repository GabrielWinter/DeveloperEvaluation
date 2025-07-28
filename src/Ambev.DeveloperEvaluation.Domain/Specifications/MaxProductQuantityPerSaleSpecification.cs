using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class MaxProductQuantityPerSaleSpecification : ISpecification<Sale>
{
    private readonly Guid _productId;
    private readonly int _addQuantity;
    private readonly int MaxAllowed = 20;

    public MaxProductQuantityPerSaleSpecification(Guid productId, int addQuantity)
    {
        _productId = productId;
        _addQuantity = addQuantity;
    }

    public bool IsSatisfiedBy(Sale sale)
    {
        var existing = sale.Items
            .Where(i => i.ProductId == _productId)
            .Sum(i => i.Quantity);

        var total = existing + _addQuantity;
        return total <= MaxAllowed;
    }
}
