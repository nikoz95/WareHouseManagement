using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.Warehouses.Commands;
using WareHouseManagement.Application.Features.Warehouses.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehousesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ყველა საწყობის მიღება
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllWarehousesQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result);
    }

    /// <summary>
    /// ახალი საწყობის შექმნა
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { }, result);
    }
}

