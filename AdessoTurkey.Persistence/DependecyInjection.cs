using AdessoTurkey.Application.Interfaces.Repositories;
using AdessoTurkey.Application.Interfaces;
using AdessoTurkey.Persistence.Data;
using AdessoTurkey.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace AdessoTurkey.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                ));

            services.AddScoped<IDrawRepository, DrawRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Otomatik migration ve seed data çalıştırır
        /// </summary>
        public static async Task ApplyMigrationsAndSeedAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                await context.Database.MigrateAsync();
                Console.WriteLine("Migration'lar başarıyla uygulandı");

                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration/Seed hatası: {ex.Message}");
                throw;
            }
        }
    }
}
