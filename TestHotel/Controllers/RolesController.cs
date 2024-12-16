using BhoomiGlobalAPI.DTO;
using BhoomiGlobalAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BhoomiGlobalAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseApiController
    {
        private IRoleService _roleService;
        private IUserDetailsService _userDetailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(IRoleService roleService, IUserDetailsService userDetailService, RoleManager<IdentityRole> roleManager)
        {
            _roleService = roleService;
            _userDetailService = userDetailService;
            _roleManager = roleManager;
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _roleService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(Int64 id)
        {
            try
            {
                var result = await _roleService.GetRoleById(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        [Route("CreatetRoles")]
        public async Task<IActionResult> CreateRoles(RoleDTO roles)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  
                    long result = 0;
                    if (roles.Id == 0)
                    {
                        if (string.IsNullOrWhiteSpace(roles.Name))
                        {
                            return BadRequest("Role name cannot be empty.");
                        }

                        var roleExists = await _roleManager.RoleExistsAsync(roles.Name);
                        if (roleExists)
                        {
                            return Conflict("Role already exists.");
                        }

                        var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole(roles.Name));
                        if (roleCreationResult.Succeeded)
                        {
                            var createdRole = await _roleManager.FindByNameAsync(roles.Name);
                            roles.RoleId = createdRole?.Id;
                            result = await _roleService.Create(roles);
                            return Ok(result);
                        }

                        return BadRequest(result);
                    }
                    else
                    {
                        result = await _roleService.Update(roles);
                    }
                    return Ok(result);
                }
                else
                {
                    return BadRequest("ModelState is not valid.");
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpsertRoles")]
        public async Task<IActionResult> UpsertRoles(RoleDTO roles)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    long result = 0;
                    if (roles.Id ==0)
                    {
                        if (string.IsNullOrWhiteSpace(roles.Name))
                        {
                            return BadRequest("Role name cannot be empty.");
                        }

                        var roleExists = await _roleManager.RoleExistsAsync(roles.Name);
                        if (roleExists)
                        {
                            return Conflict("Role already exists.");
                        }

                        var creationResult = await _roleManager.CreateAsync(new IdentityRole(roles.Name));
                        if (creationResult.Succeeded)
                        {
                            var createdRole = await _roleManager.FindByNameAsync(roles.Name);
                            roles.RoleId = createdRole?.Id;
                            result = await _roleService.Create(roles);
                            return Ok(result);
                        }

                        return BadRequest(result);
                        
                    }
                    else
                    {
                        result = await _roleService.Update(roles);  
                    }
                    return Ok(result);
                }
                else
                {
                    return BadRequest("ModelState is not valid.");
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


       
        [HttpPost]
        [Route("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(Int64 id)
        {
            try
            {
                if (id != 0)
                {
                    await _roleService.Delete(id);
                    return Ok();
                }
                else
                {
                    return BadRequest("Not Found.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }               

}
