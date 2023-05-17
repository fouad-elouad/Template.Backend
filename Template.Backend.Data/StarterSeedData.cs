using Template.Backend.Model.Entities;

namespace Template.Backend.Data
{
    /// <summary>
    /// seeding data
    /// </summary>
    public static class StarterSeedData
    {
        public static void SeedData(StarterDbContext context)
        {
            // Seed, if necessary
            if (!context.Companies.Any())
            {
                context.AddRange(GetCompanies());
                context.SaveChanges();
            }
        }

        // Company Seed
        private static List<Company> GetCompanies()
        {
            return new List<Company>
            {
                new Company {
                    Name = "A SARL",
                    CreationDate = new DateTime(2020,10,10),
                },
                new Company {
                    Name = "B SARL AU",
                    CreationDate = new DateTime(2019,01,15),
                }
            };
        }
    }
}