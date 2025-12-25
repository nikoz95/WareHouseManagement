using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.WarehouseLocations.Commands;
using WareHouseManagement.Application.Features.WarehouseLocations.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId}/locations")]
public class WarehouseLocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehouseLocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all warehouse locations for a specific warehouse
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(Guid warehouseId)
    {
        var query = new GetAllWarehouseLocationsQuery
        {
            WarehouseId = warehouseId
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new warehouse location
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(Guid warehouseId, [FromBody] CreateWarehouseLocationCommand command)
    {
        command.WarehouseId = warehouseId;
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { warehouseId }, result.Data);
    }
}

