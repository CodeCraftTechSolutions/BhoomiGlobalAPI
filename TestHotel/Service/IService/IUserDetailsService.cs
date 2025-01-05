using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;

namespace BhoomiGlobalAPI.Service.IService
{
    public interface IUserDetailsService
    {
        IEnumerable<UserDetails> GetAll();
        Task<long> GetUserDetailId(string UserId);
        Task Delete(long id);
        Task<UserDetails> GetById(long id);
        Task<bool> Update(UserDetailsUpdateDTO data);
        Task<long> AddUserDetailsOnRegister(UserRegistrationDTO userDetails);
        Task<UserDetailsData> GetUserClientProfile(string UserId);
        Task<UserDetailsData> GetUserClientProfileById(long id);
        Task<QueryResult<UserDetailsDTO>> UserDetailsList(UserDetailsQueryObject query);
        Task<List<Role>> GetRolesForAdmin();
        Task<UserDetailsDTO> GetUserClientProfileForAdmin(string UserId);
        Task<UserDetailsDTO> GetUserById(Int64 id);
        Task<UserDetails> GetUserByEmail(string email);
        List<string> RoleName(List<long> roleId);
        Task<List<Role>> GetRolesForAdminManageRole();
        Task UpdateFewerParams(UserDetailsDTO userDetailsDTO);
        Task<bool> UpdateProfilePicture(UserDetailsDTO userDetails);


    }
}
