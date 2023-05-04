using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Backend.Data
{
    /// <summary>
    /// IDbContext interface
    /// represents a combination of the Unit Of Work and Repository patterns
    /// Custom DbContext that implement this interface makes easy mocking
    /// DbContext and DbSet for unit test
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Provides access to features of the context that deal with change tracking of entities.
        /// </summary>
        DbChangeTracker ChangeTracker { get; }

        /// <summary>
        /// Provides access to configuration options for the context.
        /// </summary>
        DbContextConfiguration Configuration { get; }

        /// <summary>
        /// Creates a Database instance for this context that allows for creation/deletion/existence
        /// checks for the underlying database.
        /// </summary>
        Database Database { get; }

        /// <summary>
        /// Gets a DbEntityEntry object for the given entity
        /// providing access to information about the entity and the ability to perform actions
        /// on the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>An entry for the entity.</returns>
        DbEntityEntry Entry(object entity);

        /// <summary>
        /// Gets a DbEntityEntry object for the given
        /// entity providing access to information about the entity and the ability to perform
        /// actions on the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>An entry for the entity</returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int GetHashCode();

        Type GetType();

        /// <summary>
        /// Validates tracked entities and returns a Collection of DbEntityValidationResult
        /// containing validation results.
        /// </summary>
        /// <returns>Collection of validation results for invalid entities. The collection is never
        /// null and must not contain null values or results for valid entities.</returns>
        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are
        /// created for many-to-many relationships and relationships where there is no foreign
        /// key property included in the entity class (often referred to as independent associations).</returns>
        int SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns> A task that represents the asynchronous save operation. The task result contains
        /// the number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are
        /// created for many-to-many relationships and relationships where there is no foreign
        /// key property included in the entity class (often referred to as independent associations).</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to
        /// complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains
        /// the number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are
        /// created for many-to-many relationships and relationships where there is no foreign
        /// key property included in the entity class (often referred to as independent associations).</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns a non-generic DbSet instance for access to entities
        /// of the given type in the context and the underlying store.
        /// </summary>
        /// <param name="entityType">The type of entity for which a set should be returned.</param>
        /// <returns>A set for the given entity type.</returns>
        DbSet Set(Type entityType);

        /// <summary>
        /// Returns a DbSet instance for access to entities of the given
        /// type in the context and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity">The type entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        string ToString();

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// use connected userName for audit purpose
        /// </summary>
        /// <param name="userName">Logged user name.</param>
        int Commit(string userName);

        /// <summary>
        /// Saves Async all changes made in this context to the underlying database.
        /// use connected userName for audit purpose
        /// </summary>
        /// <param name="userName">Logged user name.</param>
        Task<int> CommitAsync(string userName);

        /// <summary>
        /// Sets the specified entity state as modified.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void SetModified(object entity);

        /// <summary>
        /// Determines whether the specified entity is detached.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is detached; otherwise, <c>false</c>.
        /// </returns>
        bool IsDetached(object entity);
    }
}
