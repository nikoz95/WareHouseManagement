﻿﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.Application.Features.Products.Commands;
using WareHouseManagement.Application.Features.Products.Queries;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());
        
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Message });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { id = result.Data?.Id }, result.Data);
    }

    /// <summary>
    /// Delete a product (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Message });

        return NoContent();
    }
}

