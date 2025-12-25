using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.Manufacturers.Commands;
using WareHouseManagement.Application.Features.Manufacturers.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/manufacturers")]
public class ManufacturersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ManufacturersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all manufacturers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllManufacturersQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new manufacturer
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateManufacturerCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { id = result.Data?.Id }, result.Data);
    }

    /// <summary>
    /// Delete a manufacturer (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteManufacturerCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Message });

        return NoContent();
    }
}

