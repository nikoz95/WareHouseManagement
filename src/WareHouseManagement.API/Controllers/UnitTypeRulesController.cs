using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.UnitTypeRules.Commands;
using WareHouseManagement.Application.Features.UnitTypeRules.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitTypeRulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitTypeRulesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ყველა საზომი ერთეულის წესის მიღება
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool? onlyActive = true)
    {
        var query = new GetAllUnitTypeRulesQuery
        {
            OnlyActive = onlyActive
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result);
    }

    /// <summary>
    /// ახალი საზომი ერთეულის წესის შექმნა
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUnitTypeRuleCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { }, result);
    }

    /// <summary>
    /// საზომი ერთეულის წესის წაშლა
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteUnitTypeRuleCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result);
    }
}

