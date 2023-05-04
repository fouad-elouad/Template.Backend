using Template.Backend.Data;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Backend.UnitTest.Configuration
{
    public class FakeDbContext : DbContext, IDbContext
    {

        /// <summary>
        /// Constructor of DBContext
        /// </summary>
        public FakeDbContext() : base()
        { }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return new FakeDbSet<TEntity>(base.Set<TEntity>());
        }

        /// <summary>
        /// SaveChanges with audit to database
        /// </summary>
        public virtual async Task CommitAsync(string userName, CancellationToken cancellationToken)
        {
            await Task.Delay(1, cancellationToken);
        }

        public void SetModified(object entity)
        {
        }

        public bool IsDetached(object entity)
        {
            return true;
        }


        public int Commit(string userName)
        {
            return 1;
        }

        public async Task<int> CommitAsync(string userName)
        {
            return await Task.FromResult(1);
        }
    }
}

