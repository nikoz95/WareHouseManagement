using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Data.Seed;

public static class AuthSeeder
{
    public static async Task SeedAuthDataAsync(ApplicationDbContext context)
    {
        // უკვე არსებობს თუ არა მონაცემები
        if (await context.Users.AnyAsync())
            return;

        // 1. უფლებების შექმნა (Permissions)
        var permissions = CreatePermissions();
        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();

        // 2. როლების შექმნა (Roles)
        var adminRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Description = "ადმინისტრატორი - სრული წვდომა ყველა რესურსზე",
            CreatedAt = DateTime.UtcNow
        };

        var guestRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Guest",
            Description = "სტუმარი - მხოლოდ ნახვის უფლება",
            CreatedAt = DateTime.UtcNow
        };

        await context.Roles.AddRangeAsync(new[] { adminRole, guestRole });
        await context.SaveChangesAsync();

        // 3. როლებზე უფლებების მინიჭება (RolePermissions)
        var rolePermissions = new List<RolePermission>();

        // Admin - ყველა უფლება
        foreach (var permission in permissions)
        {
            rolePermissions.Add(new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = adminRole.Id,
                PermissionId = permission.Id,
                CreatedAt = DateTime.UtcNow
            });
        }

        // Guest - მხოლოდ Read უფლებები
        var readPermissions = permissions.Where(p => p.Action == "Read").ToList();
        foreach (var permission in readPermissions)
        {
            rolePermissions.Add(new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = guestRole.Id,
                PermissionId = permission.Id,
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.RolePermissions.AddRangeAsync(rolePermissions);
        await context.SaveChangesAsync();

        // 4. მომხმარებლების შექმნა (Users)
        // პაროლი: Admin123! და Guest123!
        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            Email = "admin@warehouse.ge",
            FirstName = "ადმინ",
            LastName = "ადმინისტრატორი",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var guestUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "guest",
            Email = "guest@warehouse.ge",
            FirstName = "სტუმარი",
            LastName = "იუზერი",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Guest123!"),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await context.Users.AddRangeAsync(new[] { adminUser, guestUser });
        await context.SaveChangesAsync();

        // 5. მომხმარებლებზე როლების მინიჭება (UserRoles)
        var userRoles = new List<UserRole>
        {
            new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = adminUser.Id,
                RoleId = adminRole.Id,
                CreatedAt = DateTime.UtcNow
            },
            new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = guestUser.Id,
                RoleId = guestRole.Id,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.UserRoles.AddRangeAsync(userRoles);
        await context.SaveChangesAsync();
    }

    private static List<Permission> CreatePermissions()
    {
        var resources = new[]
        {
            "Product", "Company", "Warehouse", "Order", 
            "Manufacturer", "Debtor", "Stock", "User"
        };
        
        var actions = new[] { "Read", "Create", "Update", "Delete" };

        var permissions = new List<Permission>();

        foreach (var resource in resources)
        {
            foreach (var action in actions)
            {
                permissions.Add(new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = $"{resource}.{action}",
                    Description = $"{action} permission for {resource}",
                    Resource = resource,
                    Action = action,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        return permissions;
    }
}

