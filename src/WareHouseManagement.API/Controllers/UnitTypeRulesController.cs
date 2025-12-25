using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.UnitTypeRules.Commands;
using WareHouseManagement.Application.Features.UnitTypeRules.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/unit-type-rules")]
public class UnitTypeRulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitTypeRulesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all unit type rules
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] bool? onlyActive = true)
    {
        var query = new GetAllUnitTypeRulesQuery
        {
            OnlyActive = onlyActive
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new unit type rule
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUnitTypeRuleCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { id = result.Data?.Id }, result.Data);
    }

    /// <summary>
    /// Delete a unit type rule (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteUnitTypeRuleCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Message });

        return NoContent();
    }
}

