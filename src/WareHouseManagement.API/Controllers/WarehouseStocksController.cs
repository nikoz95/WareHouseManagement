using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.WarehouseStocks.Commands;
using WareHouseManagement.Application.Features.WarehouseStocks.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/warehouse-stocks")]
public class WarehouseStocksController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehouseStocksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all warehouse stocks with optional filtering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new warehouse stock entry
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseStockCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { id = result.Data?.Id }, result.Data);
    }
}

