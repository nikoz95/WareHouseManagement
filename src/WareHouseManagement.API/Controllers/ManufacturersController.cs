using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.Manufacturers.Commands;
using WareHouseManagement.Application.Features.Manufacturers.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManufacturersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ManufacturersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ყველა მწარმოებლის მიღება
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllManufacturersQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result);
    }

    /// <summary>
    /// ახალი მწარმოებლის შექმნა
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateManufacturerCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { }, result);
    }
}

