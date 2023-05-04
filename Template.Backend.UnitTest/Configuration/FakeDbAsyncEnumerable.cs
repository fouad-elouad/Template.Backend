using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Template.Backend.UnitTest.Configuration
{
    /// <summary>
    /// FakeDbAsyncEnumerable Class
    /// </summary>
    internal class FakeDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        /// <summary>
        /// FakeDbAsyncEnumerable Constructor
        /// </summary>
        /// <param name="enumerable">List of Entity</param>
        public FakeDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        /// <summary>
        /// FakeDbAsyncEnumerable constructor
        /// </summary>
        /// <param name="expression">expression</param>
        public FakeDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        /// <summary>
        /// Get Async Enumerator 
        /// </summary>
        /// <returns>IDbAsyncEnumerator</returns>
        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new FakeDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new FakeDbAsyncQueryProvider<T>(this); }
        }
    }
}