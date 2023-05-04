

namespace Template.Backend.Data.Repositories
{
    /// <summary>
    /// UnitOfWork class
    /// Manage Transactions as unit to database 
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        private IDbContext _dbContext;

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="dbFactory">Current database context</param>
        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// Return current context if not instantiate new one
        /// </summary>
        public IDbContext DbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }

        /// <summary>
        /// SaveChanges to database
        /// </summary>
        /// <param name="userName">Logged user name.</param>
        public void Commit(string userName)
        {
            DbContext.Commit(userName);
        }

    }
}
