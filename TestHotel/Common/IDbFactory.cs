namespace BhoomiGlobalAPI.Common
{
        public interface IDbFactory : IDisposable
        {
            RepositoryContext Init();
        }
    

}
