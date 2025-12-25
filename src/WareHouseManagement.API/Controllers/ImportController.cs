using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.Infrastructure.Services;

namespace WareHouseManagement.API.Controllers;

/// <summary>
/// Excel Import/Export Operations
/// </summary>
[ApiController]
[Route("api/import")]
[Produces("application/json")]
public class ImportController : ControllerBase
{
    private readonly IExcelImportService _excelImportService;
    private readonly ILogger<ImportController> _logger;

    public ImportController(IExcelImportService excelImportService, ILogger<ImportController> logger)
    {
        _excelImportService = excelImportService;
        _logger = logger;
    }

    /// <summary>
    /// Import products from Excel file
    /// </summary>
    [HttpPost("products")]
    [ProducesResponseType(typeof(ImportResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportProducts(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "ფაილი არ არის არჩეული" });

        if (!file.FileName.EndsWith(".xlsx"))
            return BadRequest(new { error = "მხოლოდ .xlsx ფაილები დაშვებულია" });

        try
        {
            using var stream = file.OpenReadStream();
            var result = await _excelImportService.ImportProductsAsync(stream);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing products");
            return BadRequest(new { error = $"შეცდომა: {ex.Message}" });
        }
    }

    /// <summary>
    /// Import companies from Excel file
    /// </summary>
    [HttpPost("companies")]
    [ProducesResponseType(typeof(ImportResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportCompanies(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "ფაილი არ არის არჩეული" });

        if (!file.FileName.EndsWith(".xlsx"))
            return BadRequest(new { error = "მხოლოდ .xlsx ფაილები დაშვებულია" });

        try
        {
            using var stream = file.OpenReadStream();
            var result = await _excelImportService.ImportCompaniesAsync(stream);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing companies");
            return BadRequest(new { error = $"შეცდომა: {ex.Message}" });
        }
    }


    /// <summary>
    /// Download Excel template for products
    /// </summary>
    [HttpGet("templates/products")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DownloadProductsTemplate()
    {
        var fileBytes = await _excelImportService.GenerateProductsTemplateAsync();
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products_Template.xlsx");
    }

    /// <summary>
    /// Download Excel template for companies
    /// </summary>
    [HttpGet("templates/companies")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DownloadCompaniesTemplate()
    {
        var fileBytes = await _excelImportService.GenerateCompaniesTemplateAsync();
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Companies_Template.xlsx");
    }
}

