namespace BhoomiGlobalAPI.Common
{
    public class Disposable : IDisposable
    {
        private bool _isDisposed;

        // Destructor calls Dispose
        ~Disposable()
        {
            Dispose(false);
        }

        // Public Dispose method
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Dispose logic that is shared
        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                // Call to DisposeCore to allow derived classes to implement custom disposal logic
                DisposeCore();
            }

            _isDisposed = true;
        }

        // Override this in derived classes to dispose custom objects
        protected virtual void DisposeCore()
        {
        }
    }
}
