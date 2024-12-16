using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BhoomiGlobal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly IUserDetailsService _bookingService;

        public UserDetailsController(IUserDetailsService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<UserDetails> GetAll()
        {
            var result = _bookingService.GetAll();
            return Ok(result);
        }


        [HttpPost]
        [Route("Delete")]

        public async Task Delete(int id)
        {
            await _bookingService.Delete(id);
        }


        [HttpGet]
        [Route("GetById")]
        public async Task<UserDetails> GetById(int id)
        {
            var result = await _bookingService.GetById(id);
            return result;
        }
    }

}
