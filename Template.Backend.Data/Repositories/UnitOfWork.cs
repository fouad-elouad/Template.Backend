

namespace Template.Backend.Data.Repositories
{
    /// <summary>
    /// UnitOfWork class
    /// Manage Transactions as unit to database 
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private StarterDbContext _dbContext;

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="dbFactory">Current database context</param>
        public UnitOfWork(StarterDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        /// SaveChanges to database
        /// </summary>
        public void Commit()
        {
            _dbContext.Commit();
        }

    }
}
