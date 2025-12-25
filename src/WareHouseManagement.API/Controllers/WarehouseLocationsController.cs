using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.WarehouseLocations.Commands;
using WareHouseManagement.Application.Features.WarehouseLocations.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseLocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehouseLocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ყველა საწყობის ლოკაციის მიღება (ან კონკრეტული საწყობის ლოკაციები)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? warehouseId = null)
    {
        var query = new GetAllWarehouseLocationsQuery
        {
            WarehouseId = warehouseId
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result);
    }

    /// <summary>
    /// ახალი საწყობის ლოკაციის შექმნა
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseLocationCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { warehouseId = result.Data?.WarehouseId }, result);
    }
}

