using Domain.Interface;
using Infrastructure.Services;
using Infrastructure.State;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddHttpClient<IFleetApi, FleetApi>(client =>
            {
                client.BaseAddress = new Uri("https://esa.instech.no/api/");
            });

            services.AddScoped<IAnchorageState, AnchorageState>();

            return services;
        }

    }
}
