﻿using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WareHouseManagement.Application.Mappings;

namespace WareHouseManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Mapperly mapper as singleton
        services.AddSingleton<ApplicationMapper>(new ApplicationMapper());
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}

