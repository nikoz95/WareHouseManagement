using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.CompanyLocations.Commands;
using WareHouseManagement.Application.Features.CompanyLocations.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyLocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyLocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ყველა კომპანიის ლოკაციის მიღება (ან კონკრეტული კომპანიის ლოკაციები)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? companyId = null)
    {
        var query = new GetAllCompanyLocationsQuery
        {
            CompanyId = companyId
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result);
    }

    /// <summary>
    /// ახალი კომპანიის ლოკაციის შექმნა
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyLocationCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { companyId = result.Data?.CompanyId }, result);
    }
}

