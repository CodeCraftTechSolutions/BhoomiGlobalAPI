using AutoMapper;
using BhoomiGlobaAPI.Repository.Infrastructure;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.IService;

namespace BhoomiGlobalAPI.Service
{
    public class UserDetailsService : IUserDetailsService
        
    {
        IUserDetailsRepository _userdetailsRepository;
        private readonly IMapper _mapper;
        IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;

        public UserDetailsService(IUserDetailsRepository userdetailsRepository,IMapper mapper, IUnitOfWork unitOfWork,
                                    IRoleRepository roleRepository)
        {
            _userdetailsRepository = userdetailsRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
        }

        public IEnumerable<UserDetails> GetAll()
        {
            var Result = _userdetailsRepository.GetAll();
            return Result;
        }

        public async Task<long> GetUserDetailId(string UserId)
        {
            long id = 0;
            if (string.IsNullOrEmpty(UserId) == true) return id;
            var result = _userdetailsRepository.FindBy(x => x.UserId == UserId).FirstOrDefault();
            if (result != null && result.Id > 0)
            {
                id = result.Id;
            }
            return id;
        }


        public async Task<long> AddUserDetailsOnRegister(UserRegistrationDTO userDetails)
        {
            try
            {
                var userdetailsData = _mapper.Map<UserDetails>(userDetails);
                userdetailsData.RegisteredDate = DateTime.Now;
                await _userdetailsRepository.Add(userdetailsData);
                await _unitOfWork.Commit();
                var UserRoleList = new List<UserRole>();
                foreach(var role in userDetails.RoleNames)
                {
                    var roleData = _roleRepository.GetAll(x => x.Name.ToLower() == role.ToLower()).FirstOrDefault();
                    if(roleData != null)
                    {
                        UserRole userRole = new UserRole()
                        {
                            UserId = roleData.Id,
                            RoleId = roleData.Id,
                        };

                        UserRoleList.Add(userRole);
                        await _unitOfWork.Commit();
                    }
                }
                await _unitOfWork.Commit();
                return userdetailsData.Id;

            }catch(Exception ex)
            {
                throw ex;
            }
        }
       

        public async Task Delete(int id)
        {
            var data = _userdetailsRepository.GetById(id);
            _userdetailsRepository.Delete(data);
            await _unitOfWork.Commit();
        }

        public async Task<UserDetails> GetById(int id)
        {
            return await _userdetailsRepository.GetSingle(id);
        }
    }
}
