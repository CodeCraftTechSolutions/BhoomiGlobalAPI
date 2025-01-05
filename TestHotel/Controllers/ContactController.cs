using BhoomiGlobalAPI.Api.Controllers;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BhoomiGlobalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : BaseApiController
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var data = _contactService.GetAll();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(ContactDTO model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var data = await _contactService.Add(model);
                    return Ok(data);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById(long id)
        {
            try
            {
               if(id > 0)
                {
                    var data = await _contactService.Delete(id);
                    return Ok(data);
                }
               return BadRequest("invalid Id.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
               if(id > 0)
                {
                    var data = _contactService.GetById(id);
                    return Ok(data);
                }
               return BadRequest("invalid Id.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
