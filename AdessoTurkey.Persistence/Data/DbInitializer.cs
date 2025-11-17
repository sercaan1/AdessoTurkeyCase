using AdessoTurkey.Domain.Entities;

namespace AdessoTurkey.Persistence.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Teams.Any())
            {
                return;
            }

            SeedTeams(context);
        }

        private static void SeedTeams(ApplicationDbContext context)
        {
            var teams = new List<Team>
            {
                new Team { Name = "Adesso İstanbul", Country = "Türkiye", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Ankara", Country = "Türkiye", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso İzmir", Country = "Türkiye", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Antalya", Country = "Türkiye", CreatedAt = DateTime.UtcNow },

                new Team { Name = "Adesso Berlin", Country = "Almanya", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Frankfurt", Country = "Almanya", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Münih", Country = "Almanya", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Dortmund", Country = "Almanya", CreatedAt = DateTime.UtcNow },

                new Team { Name = "Adesso Paris", Country = "Fransa", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Marsilya", Country = "Fransa", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Nice", Country = "Fransa", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Lyon", Country = "Fransa", CreatedAt = DateTime.UtcNow },

                new Team { Name = "Adesso Amsterdam", Country = "Hollanda", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Rotterdam", Country = "Hollanda", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Lahey", Country = "Hollanda", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Eindhoven", Country = "Hollanda", CreatedAt = DateTime.UtcNow },

                new Team { Name = "Adesso Lisbon", Country = "Portekiz", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Porto", Country = "Portekiz", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Braga", Country = "Portekiz", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Coimbra", Country = "Portekiz", CreatedAt = DateTime.UtcNow },

                new Team { Name = "Adesso Roma", Country = "İtalya", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Milano", Country = "İtalya", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Venedik", Country = "İtalya", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Napoli", Country = "İtalya", CreatedAt = DateTime.UtcNow },

                new Team { Name = "Adesso Sevilla", Country = "İspanya", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Madrid", Country = "İspanya", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Barselona", Country = "İspanya", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Granada", Country = "İspanya", CreatedAt = DateTime.UtcNow },

                new Team { Name = "Adesso Brüksel", Country = "Belçika", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Brugge", Country = "Belçika", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Gent", Country = "Belçika", CreatedAt = DateTime.UtcNow },
                new Team { Name = "Adesso Anvers", Country = "Belçika", CreatedAt = DateTime.UtcNow }
            };

            context.Teams.AddRange(teams);
            context.SaveChanges();

            Console.WriteLine($"{teams.Count} takım başarıyla seed edildi");
        }
    }
}
