using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;

namespace BhoomiGlobalAPI.Service.IService
{
    public interface IUserDetailsService
    {
        IEnumerable<UserDetails> GetAll();
        Task<long> GetUserDetailId(string UserId);
        Task Delete(int id);
        Task<UserDetails> GetById(int id);
        Task<long> AddUserDetailsOnRegister(UserRegistrationDTO userDetails);


    }
}
