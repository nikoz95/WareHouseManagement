using Microsoft.AspNetCore.Mvc;
using MediatR;
using WareHouseManagement.Application.Features.WarehouseStockHistory.Queries;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/warehouse-stocks/history")]
public class StockHistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockHistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get warehouse stock history with optional filters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? warehouseStockId = null,
        [FromQuery] Guid? productId = null,
        [FromQuery] Guid? orderId = null,
        [FromQuery] StockTransactionType? transactionType = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        var query = new GetStockHistoryQuery
        {
            WarehouseStockId = warehouseStockId,
            ProductId = productId,
            OrderId = orderId,
            TransactionType = transactionType,
            FromDate = fromDate,
            ToDate = toDate
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Message, errors = result.Errors });

        return Ok(result.Data);
    }
}

