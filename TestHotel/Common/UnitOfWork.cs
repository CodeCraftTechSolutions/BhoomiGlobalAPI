
namespace BhoomiGlobalAPI.Common
{

    public class UnitOfWork : IUnitOfWork
        {
            private readonly IDbFactory dbFactory;
            private RepositoryContext dbContext;

            public UnitOfWork(IDbFactory dbFactory)
            {
                this.dbFactory = dbFactory;
            }

            public RepositoryContext DbContext
            {
                get
                {

                    return (dbContext = dbFactory.Init());

                }
            }

            public async Task<int> Commit()
            {
                return await DbContext.SaveChangesAsync();
            }
        }
    
}
