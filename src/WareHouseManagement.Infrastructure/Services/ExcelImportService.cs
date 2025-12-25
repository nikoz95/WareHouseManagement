using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Enums;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Services;

public interface IExcelImportService
{
    Task<ImportResult> ImportProductsAsync(Stream excelStream);
    Task<ImportResult> ImportCompaniesAsync(Stream excelStream);
    Task<byte[]> GenerateProductsTemplateAsync();
    Task<byte[]> GenerateCompaniesTemplateAsync();
}

public class ImportResult
{
    public bool IsSuccess { get; set; }
    public int TotalRows { get; set; }
    public int Added { get; set; }
    public int Updated { get; set; }
    public int Skipped { get; set; }
    public List<string> Errors { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}

public class ExcelImportService : IExcelImportService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ExcelImportService> _logger;

    public ExcelImportService(ApplicationDbContext context, ILogger<ExcelImportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Products Import

    public async Task<ImportResult> ImportProductsAsync(Stream excelStream)
    {
        var result = new ImportResult();

        try
        {
            using var workbook = new XLWorkbook(excelStream);
            var worksheet = workbook.Worksheet(1);
            var rowCount = worksheet.LastRowUsed()?.RowNumber() ?? 0;

            if (rowCount <= 1)
            {
                result.Errors.Add("Excel ფაილი ცარიელია");
                return result;
            }

            result.TotalRows = rowCount - 1;

            // Get default UnitTypeRule
            var literRule = await _context.UnitTypeRules
                .FirstOrDefaultAsync(r => r.UnitType == UnitType.Liter && r.IsActive);

            if (literRule == null)
            {
                result.Errors.Add("UnitTypeRule (Liter) ვერ მოიძებნა");
                return result;
            }

            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    var name = worksheet.Cell(row, 1).GetString().Trim();
                    var description = worksheet.Cell(row, 2).GetString().Trim();
                    var barcode = worksheet.Cell(row, 3).GetString().Trim();
                    var price = worksheet.Cell(row, 4).TryGetValue(out decimal p) ? p : 0;
                    var alcoholPercentage = worksheet.Cell(row, 5).TryGetValue(out decimal alc) ? (decimal?)alc : null;
                    var region = worksheet.Cell(row, 10).GetString().Trim();
                    var servingTemp = worksheet.Cell(row, 12).GetString().Trim();
                    var qualityClass = worksheet.Cell(row, 13).GetString().Trim();

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(barcode))
                    {
                        result.Skipped++;
                        continue;
                    }

                    // Check existing
                    var existing = await _context.Products
                        .Include(p => p.ProductDetails)
                        .ThenInclude(pd => pd!.AlcoholicDetails)
                        .FirstOrDefaultAsync(p => p.Barcode == barcode);

                    if (existing != null)
                    {
                        // Update
                        existing.Name = name;
                        existing.Description = description;
                        existing.Price = price;
                        existing.UpdatedAt = DateTime.UtcNow;
                        result.Updated++;
                    }
                    else
                    {
                        // Create new
                        var newProduct = new Product
                        {
                            Name = name,
                            Description = description,
                            Barcode = barcode,
                            Price = price,
                            UnitTypeRuleId = literRule.Id
                        };

                        _context.Products.Add(newProduct);
                        await _context.SaveChangesAsync();

                        // Add details if alcoholic
                        if (alcoholPercentage.HasValue)
                        {
                            var productDetails = new ProductDetails
                            {
                                ProductId = newProduct.Id,
                                ProductType = "Alcoholic",
                                CountryOfOrigin = region,
                                AdditionalNotes = description
                            };

                            _context.ProductDetails.Add(productDetails);
                            await _context.SaveChangesAsync();

                            decimal? tempValue = null;
                            if (!string.IsNullOrWhiteSpace(servingTemp))
                            {
                                var tempStr = servingTemp.Replace("°C", "").Replace("C", "").Trim();
                                if (decimal.TryParse(tempStr, out var temp))
                                    tempValue = temp;
                            }

                            var alcoholicDetails = new AlcoholicProductDetails
                            {
                                ProductDetailsId = productDetails.Id,
                                AlcoholPercentage = alcoholPercentage.Value,
                                Region = region,
                                ServingTemperature = tempValue,
                                QualityClass = qualityClass
                            };

                            _context.Set<AlcoholicProductDetails>().Add(alcoholicDetails);
                        }

                        result.Added++;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"ხაზი {row}: {ex.Message}");
                    _logger.LogError(ex, "Error at row {Row}", row);
                }
            }

            result.IsSuccess = result.Errors.Count == 0;
            result.Message = $"დასრულდა: {result.Added} დაემატა, {result.Updated} განახლდა, {result.Skipped} გამოტოვდა";

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing products");
            result.Errors.Add($"შეცდომა: {ex.Message}");
            return result;
        }
    }

    #endregion

    #region Companies Import

    public async Task<ImportResult> ImportCompaniesAsync(Stream excelStream)
    {
        var result = new ImportResult();

        try
        {
            using var workbook = new XLWorkbook(excelStream);
            var worksheet = workbook.Worksheet(1);
            var rowCount = worksheet.LastRowUsed()?.RowNumber() ?? 0;

            if (rowCount <= 1)
            {
                result.Errors.Add("Excel ფაილი ცარიელია");
                return result;
            }

            result.TotalRows = rowCount - 1;

            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    var name = worksheet.Cell(row, 1).GetString().Trim();
                    var taxId = worksheet.Cell(row, 2).GetString().Trim();
                    var email = worksheet.Cell(row, 3).GetString().Trim();
                    var phone = worksheet.Cell(row, 4).GetString().Trim();
                    var address = worksheet.Cell(row, 5).GetString().Trim();
                    var companyTypeStr = worksheet.Cell(row, 6).GetString().Trim();
                    var isPartner = worksheet.Cell(row, 7).GetString().Trim().ToLower() is "yes" or "true" or "კი";

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(taxId))
                    {
                        result.Skipped++;
                        continue;
                    }

                    var companyType = ParseCompanyType(companyTypeStr);

                    var existing = await _context.Companies
                        .FirstOrDefaultAsync(c => c.TaxId == taxId);

                    if (existing != null)
                    {
                        existing.Name = name;
                        existing.Email = email;
                        existing.Phone = phone;
                        existing.Address = address;
                        existing.CompanyType = companyType;
                        existing.IsPartner = isPartner;
                        existing.UpdatedAt = DateTime.UtcNow;
                        result.Updated++;
                    }
                    else
                    {
                        var newCompany = new Company
                        {
                            Name = name,
                            TaxId = taxId,
                            Email = email,
                            Phone = phone,
                            Address = address,
                            CompanyType = companyType,
                            IsPartner = isPartner
                        };

                        _context.Companies.Add(newCompany);
                        result.Added++;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"ხაზი {row}: {ex.Message}");
                }
            }

            result.IsSuccess = result.Errors.Count == 0;
            result.Message = $"დასრულდა: {result.Added} დაემატა, {result.Updated} განახლდა, {result.Skipped} გამოტოვდა";

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing companies");
            result.Errors.Add($"შეცდომა: {ex.Message}");
            return result;
        }
    }

    private CompanyType ParseCompanyType(string type)
    {
        return type.ToLower() switch
        {
            "restaurant" or "რესტორანი" => CompanyType.Restaurant,
            "bar" or "ბარი" => CompanyType.Bar,
            "chain" or "ქსელი" or "chaincompany" => CompanyType.ChainCompany,
            "retail" or "საცალო" => CompanyType.Retail,
            "other" or "სხვა" => CompanyType.Other,
            _ => CompanyType.Other
        };
    }

    #endregion

    #region Template Generation

    public async Task<byte[]> GenerateProductsTemplateAsync()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Products");

        // Headers
        worksheet.Cell(1, 1).Value = "Name *";
        worksheet.Cell(1, 2).Value = "Description";
        worksheet.Cell(1, 3).Value = "Barcode *";
        worksheet.Cell(1, 4).Value = "Price";
        worksheet.Cell(1, 5).Value = "AlcoholPercentage";
        worksheet.Cell(1, 6).Value = "Volume (L)";
        worksheet.Cell(1, 7).Value = "Color";
        worksheet.Cell(1, 8).Value = "SugarContent";
        worksheet.Cell(1, 9).Value = "Manufacturer";
        worksheet.Cell(1, 10).Value = "Region";
        worksheet.Cell(1, 11).Value = "GrapeVariety";
        worksheet.Cell(1, 12).Value = "ServingTemperature";
        worksheet.Cell(1, 13).Value = "QualityClass";

        // Style header
        var headerRange = worksheet.Range(1, 1, 1, 13);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

        // Example rows
        worksheet.Cell(2, 1).Value = "Saperavi Reserve 2020";
        worksheet.Cell(2, 2).Value = "Premium Georgian Red Wine";
        worksheet.Cell(2, 3).Value = "1234567890123";
        worksheet.Cell(2, 4).Value = 45.50;
        worksheet.Cell(2, 5).Value = 13.5;
        worksheet.Cell(2, 6).Value = 0.75;
        worksheet.Cell(2, 7).Value = "Red";
        worksheet.Cell(2, 8).Value = 4.5;
        worksheet.Cell(2, 9).Value = "Kakheti Winery";
        worksheet.Cell(2, 10).Value = "Kakheti";
        worksheet.Cell(2, 11).Value = "Saperavi";
        worksheet.Cell(2, 12).Value = "16-18°C";
        worksheet.Cell(2, 13).Value = "Premium";

        worksheet.Cell(3, 1).Value = "Chacha Premium";
        worksheet.Cell(3, 2).Value = "Traditional Georgian Brandy";
        worksheet.Cell(3, 3).Value = "1234567890124";
        worksheet.Cell(3, 4).Value = 25.00;
        worksheet.Cell(3, 5).Value = 40;
        worksheet.Cell(3, 6).Value = 0.5;
        worksheet.Cell(3, 10).Value = "Georgia";
        worksheet.Cell(3, 13).Value = "Premium";

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return await Task.FromResult(stream.ToArray());
    }

    public async Task<byte[]> GenerateCompaniesTemplateAsync()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Companies");

        // Headers
        worksheet.Cell(1, 1).Value = "Name *";
        worksheet.Cell(1, 2).Value = "TaxId *";
        worksheet.Cell(1, 3).Value = "Email";
        worksheet.Cell(1, 4).Value = "Phone";
        worksheet.Cell(1, 5).Value = "Address";
        worksheet.Cell(1, 6).Value = "CompanyType (Restaurant/Bar/Chain/Retail/Other)";
        worksheet.Cell(1, 7).Value = "IsPartner (Yes/No)";

        // Style
        var headerRange = worksheet.Range(1, 1, 1, 7);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGreen;

        // Examples
        worksheet.Cell(2, 1).Value = "ღვინის კომპანია ABC";
        worksheet.Cell(2, 2).Value = "123456789";
        worksheet.Cell(2, 3).Value = "info@wineabc.ge";
        worksheet.Cell(2, 4).Value = "+995 32 2 123456";
        worksheet.Cell(2, 5).Value = "თბილისი, ვაჟა-ფშაველას 45";
        worksheet.Cell(2, 6).Value = "Retail";
        worksheet.Cell(2, 7).Value = "Yes";

        worksheet.Cell(3, 1).Value = "რესტორანი XYZ";
        worksheet.Cell(3, 2).Value = "987654321";
        worksheet.Cell(3, 3).Value = "info@restaurant.ge";
        worksheet.Cell(3, 4).Value = "+995 32 2 654321";
        worksheet.Cell(3, 5).Value = "თბილისი, რუსთაველის 10";
        worksheet.Cell(3, 6).Value = "Restaurant";
        worksheet.Cell(3, 7).Value = "No";

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return await Task.FromResult(stream.ToArray());
    }

    #endregion
}

