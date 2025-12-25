using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.CompanyLocations.Commands;
using WareHouseManagement.Application.Features.CompanyLocations.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/companies/{companyId}/locations")]
public class CompanyLocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyLocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all locations for a specific company
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(Guid companyId)
    {
        var query = new GetAllCompanyLocationsQuery
        {
            CompanyId = companyId
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new company location
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(Guid companyId, [FromBody] CreateCompanyLocationCommand command)
    {
        command.CompanyId = companyId;
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { companyId }, result.Data);
    }
}

