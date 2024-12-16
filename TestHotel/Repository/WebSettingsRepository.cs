using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.Infrastructure;


namespace BhoomiGlobalAPI.Repository.Repository
{
    public class WebSettingsRepository:RepositoryBase<WebSettings>,IWebSettingsRepository
    {
         public WebSettingsRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
