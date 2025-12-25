using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<Result<ProductDto>>
{
    public Guid Id { get; set; }
}

