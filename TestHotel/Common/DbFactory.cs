namespace BhoomiGlobalAPI.Common
{
    public class DbFactory : Disposable, IDbFactory
    {
        private RepositoryContext _dbContext;

        public DbFactory(RepositoryContext db)
        {
            _dbContext = db;
        }

        // Initialize the DbContext
        public RepositoryContext Init()
        {
            return _dbContext;
        }

        // Dispose of the DbContext in the DisposeCore method
        protected override void DisposeCore()
        {
            // Dispose the DbContext when the object is disposed
            _dbContext?.Dispose();
        }
    }
}
