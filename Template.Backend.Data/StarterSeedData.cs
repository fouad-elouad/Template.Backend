using Template.Backend.Model.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace Template.Backend.Data
{
    /// <summary>
    /// derived class for seeding the database
    /// </summary>
    class StarterSeedData : DropCreateDatabaseIfModelChanges<StarterDbContext>
    {
        /// <summary>
        /// add data to the context for seeding
        /// </summary>
        /// <param name="context">The context to seed</param>
        protected override void Seed(StarterDbContext context)
        {
            context.Companies.AddRange(GetCompanies());
            context.Commit("System Seed");
        }


        // Company Seed
        private static List<Company> GetCompanies()
        {
            return new List<Company>
            {
                new Company {
                    Name = "A SARL",
                },
                new Company {
                    Name = "B SARL AU",
                }
            };
        }
    }
}