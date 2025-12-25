using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.WarehouseStocks.Commands;
using WareHouseManagement.Application.Features.WarehouseStocks.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseStocksController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehouseStocksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ყველა საწყობის მარაგის მიღება
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? warehouseLocationId = null,
        [FromQuery] Guid? productId = null,
        [FromQuery] Guid? manufacturerId = null,
        [FromQuery] bool includePackagingDetails = false,
        [FromQuery] bool includeAlcoholicDetails = false)
    {
        var query = new GetAllWarehouseStocksQuery
        {
            WarehouseLocationId = warehouseLocationId,
            ProductId = productId,
            ManufacturerId = manufacturerId,
            IncludePackagingDetails = includePackagingDetails,
            IncludeAlcoholicDetails = includeAlcoholicDetails
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// ახალი საწყობის მარაგის დამატება
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseStockCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { }, result);
    }
}

