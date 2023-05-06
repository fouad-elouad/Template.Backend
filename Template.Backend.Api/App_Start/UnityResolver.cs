using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

namespace Template.Backend.Api
{
    /// <summary>
    /// Represents a dependency injection Unity container.
    /// </summary>
    /// <seealso cref="System.Web.Http.Dependencies.IDependencyResolver" />
    public class UnityResolver : IDependencyResolver
    {
        /// <summary>
        /// The Unity container
        /// </summary>
        protected IUnityContainer container;
        private bool _IsDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <param name="serviceType">The service to be retrieved.</param>
        /// <returns>
        /// The retrieved service.
        /// </returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        /// <returns>
        /// The retrieved collection of services.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>
        /// The dependency scope.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_IsDisposed)
                return;

            if (disposing && container != null)
                container.Dispose();

            _IsDisposed = true;
        }
    }
}