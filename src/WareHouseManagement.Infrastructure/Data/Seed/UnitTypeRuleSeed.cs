using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Infrastructure.Data.Seed;

public static class UnitTypeRuleSeed
{
    public static void SeedUnitTypeRules(this ModelBuilder modelBuilder)
    {
        var now = DateTime.UtcNow;

        modelBuilder.Entity<UnitTypeRule>().HasData(
            // ცალი - მთელი რიცხვი
            new UnitTypeRule
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                UnitType = UnitType.Piece,
                NameKa = "ცალი",
                NameEn = "Piece",
                Abbreviation = "ც",
                AllowOnlyWholeNumbers = true,
                MinValue = 1,
                MaxValue = null,
                DefaultValue = 1,
                IsActive = true,
                Description = "პროდუქტის რაოდენობა ცალების მიხედვით",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            // ლიტრი - ათწილადი რიცხვი
            new UnitTypeRule
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                UnitType = UnitType.Liter,
                NameKa = "ლიტრი",
                NameEn = "Liter",
                Abbreviation = "ლ",
                AllowOnlyWholeNumbers = false,
                MinValue = 0.001m,
                MaxValue = 1000,
                DefaultValue = 0.5m,
                IsActive = true,
                Description = "პროდუქტის მოცულობა ლიტრებში",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            // მილილიტრი - ათწილადი რიცხვი
            new UnitTypeRule
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                UnitType = UnitType.Milliliter,
                NameKa = "მილილიტრი",
                NameEn = "Milliliter",
                Abbreviation = "მლ",
                AllowOnlyWholeNumbers = false,
                MinValue = 1,
                MaxValue = 10000,
                DefaultValue = 500,
                IsActive = true,
                Description = "პროდუქტის მოცულობა მილილიტრებში",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            // კილოგრამი - ათწილადი რიცხვი
            new UnitTypeRule
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                UnitType = UnitType.Kilogram,
                NameKa = "კილოგრამი",
                NameEn = "Kilogram",
                Abbreviation = "კგ",
                AllowOnlyWholeNumbers = false,
                MinValue = 0.001m,
                MaxValue = 1000,
                DefaultValue = 1,
                IsActive = true,
                Description = "პროდუქტის წონა კილოგრამებში",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            // გრამი - ათწილადი რიცხვი
            new UnitTypeRule
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                UnitType = UnitType.Gram,
                NameKa = "გრამი",
                NameEn = "Gram",
                Abbreviation = "გ",
                AllowOnlyWholeNumbers = false,
                MinValue = 1,
                MaxValue = 100000,
                DefaultValue = 100,
                IsActive = true,
                Description = "პროდუქტის წონა გრამებში",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            // ყუთი - მთელი რიცხვი
            new UnitTypeRule
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000006"),
                UnitType = UnitType.Box,
                NameKa = "ყუთი",
                NameEn = "Box",
                Abbreviation = "ყთ",
                AllowOnlyWholeNumbers = true,
                MinValue = 1,
                MaxValue = null,
                DefaultValue = 1,
                IsActive = true,
                Description = "პროდუქტის რაოდენობა ყუთების მიხედვით",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            // პაკეტი - მთელი რიცხვი
            new UnitTypeRule
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000007"),
                UnitType = UnitType.Package,
                NameKa = "პაკეტი",
                NameEn = "Package",
                Abbreviation = "პქ",
                AllowOnlyWholeNumbers = true,
                MinValue = 1,
                MaxValue = null,
                DefaultValue = 1,
                IsActive = true,
                Description = "პროდუქტის რაოდენობა პაკეტების მიხედვით",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            }
        );
    }
}

