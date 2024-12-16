using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class UserDetailsRepository : RepositoryBase<UserDetails>,IUserDetailsRepository
    {
        public UserDetailsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
