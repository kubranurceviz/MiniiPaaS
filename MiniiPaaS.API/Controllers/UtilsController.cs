using Microsoft.AspNetCore.Mvc;
using MiniiPaaS.Application.Models; 

namespace MiniiPaaS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilsController : ControllerBase
    {
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            return Ok(RoleDto.GetAll());
        }
    }
}


