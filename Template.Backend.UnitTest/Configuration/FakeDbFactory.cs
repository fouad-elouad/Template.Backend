using Template.Backend.Data;
using Template.Backend.Data.Repositories;

namespace Template.Backend.UnitTest.Configuration
{
    /// <summary>
    /// DbFactory class
    /// Initiate and dispose context
    /// </summary>
    public class FakeDbFactory : Disposable, IDbFactory
    {
        FakeDbContext dbContext { get; set; }

        /// <summary>
        /// instantiate context
        /// </summary>
        /// <returns>New context</returns>
        public IDbContext Init()
        {
            ///
            return dbContext ?? (dbContext = new FakeDbContext());
        }

        /// <summary>
        /// Dispose objects relating to current context 
        /// </summary>
        protected override void DisposeCore()
        {
            ///
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
