using System;


namespace Template.Backend.Data.Repositories
{
    /// <summary>
    /// Disposable class
    /// </summary>
    public class Disposable : IDisposable
    {
        private bool _isDisposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~Disposable()
        {
            Dispose(false);
        }
        /// <summary>
        /// releasing unmanaged resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// releasing unmanaged resources
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisposeCore();
            }

            _isDisposed = true;
        }

        /// <summary>
        /// Ovveride this to dispose custom objects
        /// </summary>
        protected virtual void DisposeCore()
        {
        }
    }
}
