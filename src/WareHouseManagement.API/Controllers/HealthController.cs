using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(ApplicationDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Simple health check - სისტემის მდგომარეობის შემოწმება
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            message = "WareHouse Management API is running"
        });
    }

    /// <summary>
    /// Detailed health check with database connectivity - დეტალური ჯანმრთელობის შემოწმება
    /// </summary>
    [HttpGet("detailed")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDetailed()
    {
        var healthStatus = new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            service = "WareHouse Management API",
            version = "1.0.0",
            checks = new
            {
                api = new { status = "Healthy", message = "API is running" },
                database = await CheckDatabaseHealth()
            }
        };

        return Ok(healthStatus);
    }

    /// <summary>
    /// Database health check only - მონაცემთა ბაზის შემოწმება
    /// </summary>
    [HttpGet("database")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckDatabase()
    {
        var dbHealth = await CheckDatabaseHealth();
        
        if (dbHealth.status == "Healthy")
        {
            return Ok(dbHealth);
        }
        
        return StatusCode(503, dbHealth); // Service Unavailable
    }

    private async Task<dynamic> CheckDatabaseHealth()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (!canConnect)
            {
                return new
                {
                    status = "Unhealthy",
                    message = "Cannot connect to database",
                    timestamp = DateTime.UtcNow
                };
            }

            // Try to execute a simple query
            var productCount = await _context.Products.CountAsync();
            
            return new
            {
                status = "Healthy",
                message = "Database connection successful",
                timestamp = DateTime.UtcNow,
                details = new
                {
                    connectionState = "Connected",
                    productCount = productCount
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            
            return new
            {
                status = "Unhealthy",
                message = "Database health check failed",
                error = ex.Message,
                timestamp = DateTime.UtcNow
            };
        }
    }
}

