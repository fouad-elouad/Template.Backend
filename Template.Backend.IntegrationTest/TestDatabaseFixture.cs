using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Template.Backend.Data;
using Template.Backend.Data.Utilities;
using Template.Backend.IntegrationTest.FakeServices;

namespace Template.Backend.IntegrationTest
{
    public class TestDatabaseFixture
    {
        private static readonly object _lock = new();

        public TestDatabaseFixture()
        {
        }

        public void Initialize()
        {
            lock (_lock)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }
            }
        }

        public StarterDbContext CreateContext()
        {
            var host = Host.CreateDefaultBuilder().Build();
            var config = host.Services.GetRequiredService<IConfiguration>();

            ICurrentUserService fakeUserService = new FakeUserService();
            IDateTime fakeDateTimeService = new FakeDateTimeService();
            StarterDbContext dbContext = new StarterDbContext(
                new DbContextOptionsBuilder<StarterDbContext>()
                    .UseSqlServer(config.GetConnectionString("DefaultConnection"))
                    .Options, new AuditSaveChangesInterceptor(fakeUserService, fakeDateTimeService)
                    , fakeUserService,
                     fakeDateTimeService);

            return dbContext;
        }

        public int Seed<T>(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                return 0;

            using (var context = CreateContext())
            {
                foreach (T entity in entities)
                {
                    context.Add(entity);
                }
                context.SaveChanges();
            }

            return entities.Count();
        }
    }
}