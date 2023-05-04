
namespace Template.Backend.Data.Repositories
{
    /// <summary>
    /// DbFactory class
    /// Initiate and dispose context
    /// </summary>
    public class DbFactory : Disposable, IDbFactory
    {
        private IDbContext dbContext { get; set; }

        /// <summary>
        /// instantiate context
        /// </summary>
        /// <returns>New context</returns>
        public IDbContext Init()
        {
            return dbContext ?? (dbContext = new StarterDbContext());
        }

        /// <summary>
        /// Dispose objects relating to current context 
        /// </summary>
        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
