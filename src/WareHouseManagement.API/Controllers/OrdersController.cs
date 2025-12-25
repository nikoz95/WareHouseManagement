﻿﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.Application.Features.Orders.Commands;
using WareHouseManagement.Application.Features.Orders.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all orders with optional filtering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? companyId = null,
        [FromQuery] Domain.Enums.OrderStatus? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        var query = new GetAllOrdersQuery
        {
            CompanyId = companyId,
            Status = status,
            FromDate = fromDate,
            ToDate = toDate
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetOrderByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Message });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
    }
}

