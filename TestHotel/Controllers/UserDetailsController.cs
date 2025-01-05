using BhoomiGlobalAPI.Api.Controllers;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Service;
using BhoomiGlobalAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using static BhoomiGlobalAPI.HelperClass.Enums;
using Role = BhoomiGlobalAPI.Entities.Role;

namespace BhoomiGlobal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : BaseApiController
    {
        private readonly IUserDetailsService _bookingService;
        private readonly IUserDetailsService _userDetailsService;
        IWebHostEnvironment host;

        public UserDetailsController(IUserDetailsService bookingService, IUserDetailsService userDetailsService,
                                    IWebHostEnvironment environment)
        {
            _bookingService = bookingService;
            _userDetailsService = userDetailsService;
            host = environment;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<UserDetails> GetAll()
        {
            var result = _bookingService.GetAll();
            return Ok(result);
        }


        [HttpPost]
        [Route("Delete/{id}")]

        public async Task Delete(long id)
        {
            await _bookingService.Delete(id);
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<UserDetails> GetById(long id)
        {
            var result = await _bookingService.GetById(id);
            return result;
        }


        [HttpPost]
        [Route("update")]
        public async Task<bool> update(UserDetailsUpdateDTO data)
        {
            if(data.Id > 0)
            {
                var result = await _bookingService.Update(data);
                return result;
            }
            return false;
        }

        [HttpGet]
        [Route("UserClient")]
        public async Task<IActionResult> UserClientProfile()
        {
            try
            {
                
                var users = await _userDetailsService.GetUserClientProfile(GetUserId());
                return Ok(users);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("UserClientById/{id}")]
        public async Task<IActionResult> UserClientProfileById(long id)
        {
            try
            {
                
                var users = await _userDetailsService.GetUserClientProfileById(id);
                return Ok(users);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Route("UserDetailsList")]
        [HttpPost]
        public async Task<IActionResult> UserDetailsList(UserDetailsQueryObject query)
        {
            try
            {
                var result = await _userDetailsService.UserDetailsList(query);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]  
        [Route("GetUserDetailsById")]
        public async Task<IActionResult> GetUserDetailsById()
        {
            try
            {
                var uId = GetUserId();
                var userid = await _userDetailsService.GetUserDetailId(uId);
                if(userid == 0) return NotFound();
                var user =await _userDetailsService.GetById(userid);
                if (!string.IsNullOrEmpty(user.ImagePath)) { user.ImagePath = "Profile//" + user.ImagePath; }
                else { user.ImagePath = "Profile//blank-profile-image.png"; }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong while trying to get User's Details.");
            }
        }

        [HttpGet]
        [Route("GetUserDetailsByIdForAdminProfile")]
        public async Task<IActionResult> GetUserDetailsByIdForAdminProfile()
        {
            try
            {
                var user = await _userDetailsService.GetUserClientProfileForAdmin(GetUserId());
                if (!string.IsNullOrEmpty(user.ImagePath)) { user.ImagePath = "Profile//" + user.ImagePath; }
                else { user.ImagePath = "Profile//blank-profile-image.png"; }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong while trying to get User's Details.");
            }
        }

        [HttpGet]
        [Route("GetGenderEnum")]
        public async Task<IActionResult> GetGenderEnum()
        {
            try
            {
                Enums enums = new Enums();
                var result = enums.ExportEnum<GenderEnum>();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("RoleListForAdmin")]
        public async Task<IActionResult> RoleListForAdmin()
        {
            List<BhoomiGlobalAPI.Entities.Role> roleList = new List<BhoomiGlobalAPI.Entities.Role>();
            roleList = await _userDetailsService.GetRolesForAdmin();
            var roleListGroup = roleList.GroupBy(x => x.GroupNo, (key, g) => new { GroupId = key, Roles = g.ToList() }).ToList();
            return Ok(roleListGroup);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _userDetailsService.GetUserById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("CheckIfEmailExists")]
        public async Task<IActionResult> CheckIfEmailExists(CheckEmailExists model)
        {
            try
            {
                var user = await _userDetailsService.GetUserByEmail(model.Email);
                if (user != null)
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("RoleListForAdminManage")]
        public async Task<IActionResult> RoleListForAdminManage()
        {
            List<BhoomiGlobalAPI.Entities.Role> roleList = new List<BhoomiGlobalAPI.Entities.Role>();
            roleList = await _userDetailsService.GetRolesForAdminManageRole();
            var roleListGroup = roleList.GroupBy(x => x.GroupNo, (key, g) => new { GroupId = key, Roles = g.ToList() }).ToList();
            return Ok(roleListGroup);
        }

        [HttpPost]
        [Route("UpdateUserDetailAdminProfile")]
        public async Task<IActionResult> UpdateUserDetailAdminProfile(UserDetailsDTO userDetails)
        {
            try
            {
                await _userDetailsService.UpdateFewerParams(userDetails);
                var user = await _userDetailsService.GetUserClientProfileForAdmin(GetUserId());
                if (!string.IsNullOrEmpty(user.ImagePath)) { user.ImagePath = "Profile/" + user.ImagePath; }
                else { user.ImagePath = "Profile/blank-profile-image.png"; }
                return Ok(user);
            }
            catch (Exception e)
            {
                e = new Exception();
                return BadRequest("Something went wrong while updating User Details");
            }
        }

        [HttpPost]
        [Route("ChangeProfilePicture")]
        public async Task<IActionResult> ChangeProfilePicture()
        {
            try
            {
                var image = Request.Form.Files;
                var user = await _userDetailsService.GetUserClientProfileForAdmin(GetUserId());
                //UserProfileImagePathDTO userImage = _userDetailService.GetUserProfileImagePath(user.Id);
                string uploadFolderPath = Path.Combine(host.ContentRootPath, "Profile");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }
                /*Delete existing profile picture if it exists:*/
                if (!string.IsNullOrEmpty(user.ImagePath))
                {
                    var fileInfo = (uploadFolderPath + "\\" + user.ImagePath);
                    System.IO.File.Delete(fileInfo);
                }
                var filename = string.Empty;
                if (image.FirstOrDefault() != null)
                {
                    filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FirstOrDefault().FileName);
                    user.ImagePath = filename;
                    var filepath = Path.Combine(uploadFolderPath, filename);
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await image.FirstOrDefault().CopyToAsync(stream);
                        await _userDetailsService.UpdateProfilePicture(user);
                    }
                }

                user.ImagePath = "Profile" + "\\" + user.ImagePath;
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong while upading profile picture");
            }
        }

    }

}
