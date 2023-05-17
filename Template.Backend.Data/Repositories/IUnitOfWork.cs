

namespace Template.Backend.Data.Repositories
{
    /// <summary>
    /// IUnitOfWork interface
    /// Manage Transactions as unit to database
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// SaveChanges to database
        /// </summary>
        void Commit();
    }
}