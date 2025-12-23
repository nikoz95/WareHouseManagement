using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Products.Queries;

public class GetAllProductsQuery : IRequest<Result<IEnumerable<ProductDto>>>
{
}

