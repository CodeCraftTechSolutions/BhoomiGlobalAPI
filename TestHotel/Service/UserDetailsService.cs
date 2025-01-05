using AutoMapper;
using BhoomiGlobaAPI.Repository.Infrastructure;
using BhoomiGlobaAPI.Repository.Repository;
using BhoomiGlobal.Service.Extension;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.IService;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BhoomiGlobalAPI.Service
{
    public class UserDetailsService : IUserDetailsService
        
    {
        IUserDetailsRepository _userdetailsRepository;
        private readonly IMapper _mapper;
        IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserDetailsService(IUserDetailsRepository userdetailsRepository,IMapper mapper, IUnitOfWork unitOfWork,
                                    IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userdetailsRepository = userdetailsRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
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

                        await _userRoleRepository.Add(userRole);
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
       

        public async Task Delete(long id)
        {
            var data = _userdetailsRepository.GetById(id);
            _userdetailsRepository.Delete(data);
            await _unitOfWork.Commit();
        }

        public async Task<UserDetails> GetById(long id)
        {
            return await _userdetailsRepository.GetSingle(id);
        }


        public async Task<List<Role>> GetRolesForAdmin()
        {
            return await _roleRepository.All.Where(x => x.IsVisible == true).ToListAsync(); //only getting Administration for create user
        }


        public async Task<UserDetailsDTO> GetUserClientProfileForAdmin(string UserId)
        {
            if (string.IsNullOrEmpty(UserId) == false)
            {
                var userDetails = _userdetailsRepository.FindBy(x => x.UserId == UserId)
                                  .FirstOrDefault();

                DateTime? nulldate = null;
                if (userDetails != null)
                {
                    var result = new UserDetailsDTO
                    {
                        //UserAddress = _mapper.Map<List<UserAddressDTO>>(userDetails.UserAddress),
                        Id = userDetails.Id,
                        Title = userDetails.Title,
                        FName = userDetails.FName,
                        LName = userDetails.LName,
                        Email = userDetails.Email,
                        ImagePath = userDetails.ImagePath,
                        DateOfBirth = userDetails.DateOfBirth.ToString(),
                        Gender = userDetails.Gender,
                        PhoneNumber = userDetails.PhoneNumber
                    };
                    return result;
                }
                else
                {
                    return new UserDetailsDTO();
                }
            }
            return new UserDetailsDTO();
        }

        public async Task<bool> Update(UserDetailsUpdateDTO data)
        {
            var userDetailsData = await _userdetailsRepository.GetSingle(data.Id);
            if (userDetailsData != null)
            {
                userDetailsData.FName = data.FName;
                userDetailsData.LName = data.LName;
                userDetailsData.PhoneNumber = data.PhoneNumber;
                userDetailsData.Address = data.Address;
                userDetailsData.Age = data.Age ?? 0;
                userDetailsData.Gender = data.Gender;
                await _unitOfWork.Commit();
                return true;
            }
            return false;
        }

        public async Task<UserDetailsDTO> GetUserById(Int64 id)
        {
            var result = await _userdetailsRepository.FindBy(x => x.Id == id)
                        .Include(x => x.UserRole).FirstOrDefaultAsync();
            if (result == null) return new UserDetailsDTO();

            var userDetails = _mapper.Map<UserDetailsDTO>(result);
            userDetails.RoleList = result.UserRole.Select(x => x.RoleId).ToList();
            userDetails.RoleListStr = _roleRepository.FindBy(x => userDetails.RoleList.Contains(x.Id))
                                      .Select(x => x.RoleId).ToList();

            return userDetails;

        }

        public List<string> RoleName(List<long> roleId)
        {
            var role = _roleRepository.All.Where(x => roleId.Contains(x.Id));
            return role.Select(x => x.Name).ToList();
        }


        public async Task<UserDetails> GetUserByEmail(string email)
        {
            return await _userdetailsRepository.All.FirstOrDefaultAsync(x => x.Email == email);
        }


        public async Task<List<Role>> GetRolesForAdminManageRole()
        {
            return await _roleRepository.All.Where(x => x.IsVisible != false).OrderBy(x => x.GroupNo).ThenBy(x => x.OrderBy).ToListAsync();
        }

        public async Task UpdateFewerParams(UserDetailsDTO userDetailsDTO)
        {
            var user = await _userdetailsRepository.GetSingle(userDetailsDTO.Id);
            if (user != null)
            {
                user.FName = userDetailsDTO.FName;
                user.LName = userDetailsDTO.LName;
                user.PhoneNumber = userDetailsDTO.PhoneNumber;
                user.Email = userDetailsDTO.Email;
                user.DateOfBirth = Convert.ToDateTime(userDetailsDTO.DateOfBirth);
                user.Gender = userDetailsDTO.Gender;
                await _unitOfWork.Commit();
            }
        }


        public async Task<UserDetailsData> GetUserClientProfile(string UserId)
        {
            if (string.IsNullOrEmpty(UserId) == false)
            {
                var userDetails = _userdetailsRepository.FindBy(x => x.UserId == UserId)
                                  .FirstOrDefault();
                var isAffiliate = _userRoleRepository.FindBy(x => x.UserId == userDetails.Id && x.RoleId == 1);

                if (userDetails != null)
                {
                    var result = new UserDetailsData
                    {
                        Id = userDetails.Id,
                        FirstName = userDetails.FName,
                        LastName = userDetails.LName,
                        Email = userDetails.Email,
                        ImagePath = !string.IsNullOrEmpty(userDetails.ImagePath) ? "Profile/" + userDetails.ImagePath : string.Empty
                    };
                    return result;
                }
                else
                {
                    return new UserDetailsData();
                }
            }
            return new UserDetailsData();
        }
        
        public async Task<UserDetailsData> GetUserClientProfileById(long id)
        {
            if (id > 0)
            {
                var userDetails = _userdetailsRepository.FindBy(x => x.Id == id)
                                  .FirstOrDefault();

                if (userDetails != null)
                {
                    var result = new UserDetailsData
                    {
                        Id = userDetails.Id,
                        FirstName = userDetails.FName,
                        LastName = userDetails.LName,
                        Email = userDetails.Email,
                        PhoneNumber = userDetails.PhoneNumber,
                        ImagePath = !string.IsNullOrEmpty(userDetails.ImagePath) ? "Profile/" + userDetails.ImagePath : string.Empty
                    };
                    return result;
                }
                else
                {
                    return new UserDetailsData();
                }
            }
            return new UserDetailsData();
        }



        public async Task<QueryResult<UserDetailsDTO>> UserDetailsList(UserDetailsQueryObject query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "Id";
                query.IsSortAsc = false;
            }

            var columnMap = new Dictionary<string, Expression<Func<UserDetails, object>>>
            {
                ["Id"] = p => p.Id,
                ["FirstName"] = p => p.FName,
                ["LastName"] = p => p.LName,
                ["PhoneNumber"] = p => p.PhoneNumber,
                ["Email"] = p => p.Email,
            };

            var users = _userdetailsRepository.AllIncluding(x => x.UserRole);

            if (!string.IsNullOrEmpty(query.Filtertext))
            {
                query.Filtertext = query.Filtertext.ToLower();
                if (!string.IsNullOrEmpty(query.Filtertext))
                {
                    users = users
                        .Where(x => x.FName.ToLower().Contains(query.Filtertext)
                          || x.LName.ToLower().Contains(query.Filtertext)
                          || x.Email.ToLower().Contains(query.Filtertext)
                          || x.PhoneNumber.ToLower().Contains(query.Filtertext));
                }

            }



            //if (query.RoleId > 0)
            //{
            //    users = users.Where(x => x.UserRole.Any(y => y.RoleId == query.RoleId));
            //}

            //if (query.UserStatusId == 1) //Registered
            //{
            //    users = users.Where(x => x.Status.HasValue && x.Status.Value == (int)UserStatus.Registered);
            //}
            //else if (query.UserStatusId == 2) //Email Verified
            //{
            //    users = users.Where(x => x.Status.HasValue && x.Status.Value == (int)UserStatus.EmailVerified);
            //}
            //else if (query.UserStatusId == 3) //Unlocked
            //{
            //    users = users.Where(x => x.IsUserLocked.HasValue && x.IsUserLocked.Value == false);
            //}
            //else if (query.UserStatusId == 4) //Locked
            //{
            //    users = users.Where(x => x.IsUserLocked.HasValue && x.IsUserLocked.Value == true);
            //}
            //else if (query.UserStatusId == 5) //Registered + Locked
            //{
            //    users = users.Where(x => x.Status.HasValue && x.Status.Value == (int)UserStatus.Registered && x.IsUserLocked.HasValue && x.IsUserLocked.Value == true);
            //}
            //else if (query.UserStatusId == 6) //Registered + Unlocked
            //{
            //    users = users.Where(x => x.Status.HasValue && x.Status.Value == (int)Enums.UserStatus.Registered && x.IsUserLocked.HasValue && x.IsUserLocked.Value == true);
            //}
            //else if (query.UserStatusId == 7) //Email Verified + Locked
            //{
            //    users = users.Where(x => x.Status.HasValue && x.Status.Value == (int)Enums.UserStatus.EmailVerified && x.IsUserLocked.HasValue && x.IsUserLocked.Value == true);
            //}
            //else if (query.UserStatusId == 8) //Email Verified + Unlocked
            //{
            //    users = users.Where(x => x.Status.HasValue && x.Status.Value == (int)Enums.UserStatus.EmailVerified && x.IsUserLocked.HasValue && x.IsUserLocked.Value == false);
            //}

            var result = await users.ApplyOrdering(query, columnMap).ToListAsync();
            var userdetails = _mapper.Map<List<UserDetailsDTO>>(result);


            foreach (var item in userdetails)
            {
                var userrole = users.FirstOrDefault(x => x.Id == item.Id).UserRole;
                if (userrole != null)
                {
                    var list = userrole.Select(r => r.RoleId).ToList();
                    var rolelist = _roleRepository.FindBy(x => list.Contains(x.Id));
                    var role = rolelist.Select(x => x.Name).ToList();
                    item.RoleNames = string.Join(",", role);
                }
            }

            var queryResult = new QueryResult<UserDetailsDTO>
            {
                TotalItems = users.Count(),
                Items = userdetails
            };
            return queryResult;

        }


        public async Task<bool> UpdateProfilePicture(UserDetailsDTO userDetails)
        {
            try
            {
                var user = _userdetailsRepository.GetById(x => x.Id == userDetails.Id);
                user.ImagePath = userDetails.ImagePath;
                await _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
