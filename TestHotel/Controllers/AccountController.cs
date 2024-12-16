using AutoMapper;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BhoomiGlobalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserDetailsService _userDetailsService;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<User> userManager, IMapper mapper, IUserDetailsService userDetailsService,
                                SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userDetailsService = userDetailsService;
            _configuration = configuration;
            _signInManager = signInManager;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userRegistration)
        {
            try
            {
                if (userRegistration is null)
                    return BadRequest();

                var user = _mapper.Map<User>(userRegistration);
                var result = await _userManager.CreateAsync(user, userRegistration.ConfirmPassword);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { Succeed = false, Errors = errors });
                }

                userRegistration.RoleNames = new List<string> { EnumHelper.GetDescription(Enums.Role.GENERAL) };

                await _userManager.AddToRoleAsync(user, userRegistration.RoleNames.FirstOrDefault());
                userRegistration.UserId = user.Id;
                var UserResult = await _userDetailsService.AddUserDetailsOnRegister(userRegistration);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user, userRoles);
                return Ok(new { token = token, username = user.FName + " " + user.LName });
            }
            return Unauthorized("Invalid login attempt.");
        }

        private string GenerateJwtToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var userRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims : claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
