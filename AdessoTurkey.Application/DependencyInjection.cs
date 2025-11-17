using System.Reflection;
using AdessoTurkey.Application.Interfaces.Services;
using AdessoTurkey.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AdessoTurkey.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDrawService, DrawService>();
            services.AddSingleton<IRandomService, RandomService>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
