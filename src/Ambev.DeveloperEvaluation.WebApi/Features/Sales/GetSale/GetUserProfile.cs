using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class GetUserProfile : Profile
{

    public GetUserProfile()
    {
        CreateMap<Guid, Application.Sales.DeleteSale.DeleteSaleCommand>()
            .ConstructUsing(id => new Application.Sales.DeleteSale.DeleteSaleCommand(id));

        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<GetSaleItemResult, GetSaleItemResponse>();
    }
}
