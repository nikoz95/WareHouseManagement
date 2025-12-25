using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.Warehouses.Commands;
using WareHouseManagement.Application.Features.Warehouses.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehousesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all warehouses
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllWarehousesQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }

    /// <summary>
    /// Get warehouse by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetWarehouseByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Message });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new warehouse
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { id = result.Data?.Id }, result.Data);
    }

    /// <summary>
    /// Delete a warehouse (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteWarehouseCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Message });

        return NoContent();
    }
}

