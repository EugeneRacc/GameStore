using DAL.Data;
using DAL.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            await AddTestDataAsync(services.GetRequiredService<GameStoreDbContext>());
        }

        public static async Task AddTestDataAsync(GameStoreDbContext context)
        {
            if (!context.Genres.Any())
            {
                var strategyGuid = Guid.NewGuid();
                var actionGuid = Guid.NewGuid();
                var listOfGenre = new List<Genre>()
                {
                    new Genre { Id = strategyGuid, Title = "Strategy" },
                    new Genre { Id = Guid.NewGuid(), Title = "Rally", ParentId = strategyGuid },
                    new Genre { Id = Guid.NewGuid(), Title = "Arcade", ParentId = strategyGuid },
                    new Genre { Id = Guid.NewGuid(), Title = "Formula", ParentId = strategyGuid },
                    new Genre { Id = Guid.NewGuid(), Title = "Off-road", ParentId = strategyGuid },
                    new Genre { Id = Guid.NewGuid(), Title = "RPG" },
                    new Genre { Id = Guid.NewGuid(), Title = "Sports" },
                    new Genre { Id = Guid.NewGuid(), Title = "Races" },
                    new Genre { Id = actionGuid, Title = "Action" },
                    new Genre { Id = Guid.NewGuid(), Title = "FPS", ParentId = actionGuid },
                    new Genre { Id = Guid.NewGuid(), Title = "TPS", ParentId = actionGuid },
                    new Genre { Id = Guid.NewGuid(), Title = "Misc", ParentId = actionGuid },
                    new Genre { Id = Guid.NewGuid(), Title = "Adventure" },
                    new Genre { Id = Guid.NewGuid(), Title = "Puzzle&Skill" },
                    new Genre { Id = Guid.NewGuid(), Title = "Other" },
                };
                await context.Genres.AddRangeAsync(listOfGenre);
                await context.SaveChangesAsync();
            }
        }
    }
}
