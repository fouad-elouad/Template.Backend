using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Backend.UnitTest.Configuration
{
    /// <summary>
    /// FakeDbAsyncQueryProvider generic Class
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    internal class FakeDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        /// <summary>
        /// FakeDbAsyncQueryProvider Constructor 
        /// </summary>
        /// <param name="inner">IQueryProvider</param>
        internal FakeDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// CreateQuery with expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>IQueryable</returns>
        public IQueryable CreateQuery(Expression expression)
        {
            return new FakeDbAsyncEnumerable<TEntity>(expression);
        }

        /// <summary>
        /// CreateQuery with expression for an item
        /// </summary>
        /// <typeparam name="TElement">TElement</typeparam>
        /// <param name="expression">Expression</param>
        /// <returns></returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new FakeDbAsyncEnumerable<TElement>(expression);
        }

        /// <summary>
        /// Execute 
        /// </summary>
        /// <param name="expression">Expresion</param>
        /// <returns>an Object</returns>
        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        /// <summary>
        /// Generic Execute
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns></returns>
        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        /// <summary>
        /// Generic Execute 
        /// </summary>
        /// <typeparam name="TResult">TResult</typeparam>
        /// <param name="expression">Expression</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }
}
