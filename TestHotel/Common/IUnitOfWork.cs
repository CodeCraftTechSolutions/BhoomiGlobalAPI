namespace BhoomiGlobalAPI.Common
{
    public interface IUnitOfWork
    {
            Task<int> Commit();

    }
    
}
