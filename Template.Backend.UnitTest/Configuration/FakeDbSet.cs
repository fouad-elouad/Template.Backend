using Template.Backend.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Template.Backend.UnitTest.Configuration
{
    /// <summary>
    /// DbSet Mock
    /// </summary>
    public class FakeDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>, IDbAsyncEnumerable<TEntity>
        where TEntity : class
    {
        ObservableCollection<TEntity> _data;
        IQueryable _query;
        private DbSet<TEntity> dbSet;

        /// <summary>
        /// Constructor
        /// </summary>
        public FakeDbSet() : base()
        {
            _data = new ObservableCollection<TEntity>();
            _query = _data.AsQueryable();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FakeDbSet(DbSet<TEntity> dbSet)
        {
            this.dbSet = dbSet;
            _data = new ObservableCollection<TEntity>();
            _query = _data.AsQueryable();
        }

        /// <summary>
        /// Fake Add 
        /// </summary>
        public override TEntity Add(TEntity item)
        {
            _data.Add(item);
            ((IEntity)item).ID =  _data.Count();
            return item;
        }

        /// <summary>
        /// Fake Remove
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Item</returns>
        public override TEntity Remove(TEntity item)
        {
            _data.Remove(item);
            return item;
        }

        public override TEntity Attach(TEntity item)
        {
            _data.Add(item);
            ((IEntity)item).ID = _data.Count();
            return item;
        }

        /// <summary>
        /// Fake Create
        /// </summary>
        /// <returns>Entity</returns>
        public override TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        /// <summary>
        /// Fake Create
        /// </summary>
        /// <returns>TDerivedEntity</returns>
        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override ObservableCollection<TEntity> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new FakeDbAsyncQueryProvider<TEntity>(_query.Provider); }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
        {
            return new FakeDbAsyncEnumerator<TEntity>(_data.GetEnumerator());
        }

        /// <summary>
        /// Fake Find
        /// </summary>
        /// <returns>Entity</returns>
        public override TEntity Find(params object[] keyValues)
        {
            var id = (int) keyValues.Single();
            return this.SingleOrDefault(b => ((IEntity)b).ID == id);
        }

        /// <summary>
        /// FindAsync
        /// </summary>
        /// <param name="keyValues">keyValues</param>
        /// <returns>Task</returns>
        public override Task<TEntity> FindAsync(params object[] keyValues)
        {
            var id = (int) keyValues.Single();
            return this.SingleOrDefaultAsync(b => ((IEntity)b).ID == id);
        }
    }
}