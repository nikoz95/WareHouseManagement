using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.Debtors.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/debtors")]
public class DebtorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DebtorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all debtors with optional company filter
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? companyId = null)
    {
        var query = new GetAllDebtorsQuery
        {
            CompanyId = companyId
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }
}

