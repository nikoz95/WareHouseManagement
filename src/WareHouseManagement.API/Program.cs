using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WareHouseManagement.Application;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Infrastructure;
using WareHouseManagement.Infrastructure.Data;
using WareHouseManagement.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure JWT Settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var jwtSecret = jwtSettings.GetValue<string>("Secret") ?? throw new InvalidOperationException("JWT Secret is not configured");
var jwtIssuer = jwtSettings.GetValue<string>("Issuer") ?? "WareHouseManagement";
var jwtAudience = jwtSettings.GetValue<string>("Audience") ?? "WareHouseManagement";

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Development only
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    // Resource-based policies
    var resources = new[] { "Product", "Company", "Warehouse", "Order", "Manufacturer", "Debtor", "Stock", "User" };
    var actions = new[] { "Read", "Create", "Update", "Delete" };

    foreach (var resource in resources)
    {
        foreach (var action in actions)
        {
            var policyName = $"{resource}.{action}";
            options.AddPolicy(policyName, policy =>
                policy.RequireClaim("permission", policyName));
        }
    }
});

// Configure Swagger with JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "WareHouse Management API", 
        Version = "v1",
        Description = "API for Warehouse Management System with JWT Authentication"
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Application and Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Run database migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Starting database migration...");
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Apply pending migrations
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying {Count} pending migration(s)...", pendingMigrations.Count());
            await context.Database.MigrateAsync();
            logger.LogInformation("✅ Database migrations applied successfully.");
        }
        else
        {
            logger.LogInformation("✅ Database is up to date. No pending migrations.");
        }
        
        // Seed initial data
        logger.LogInformation("Checking seed data...");
        await DatabaseSeeder.SeedAsync(context);
        logger.LogInformation("✅ Seed data checked/applied successfully.");
        
        // Seed authentication data (Users, Roles, Permissions)
        logger.LogInformation("Checking auth seed data...");
        await AuthSeeder.SeedAuthDataAsync(context);
        logger.LogInformation("✅ Auth seed data checked/applied successfully.");
        logger.LogInformation("📝 Default users created:");
        logger.LogInformation("   Admin: username=admin, password=Admin123!");
        logger.LogInformation("   Guest: username=guest, password=Guest123!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ An error occurred while migrating the database.");
        // Don't throw - let the app start anyway so developers can see the error
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WareHouse Management API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Add Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

