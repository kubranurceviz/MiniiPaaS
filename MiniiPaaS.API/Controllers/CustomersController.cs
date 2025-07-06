using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniiPaaS.Application.Commands.CRM;
using MiniiPaaS.Application.Queries;
using MiniiPaaS.Domain.Entities;

namespace MiniiPaaS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            // Giriş yapan kullanıcının CompanyId’sini buradan al
            command.CompanyId = int.Parse(User.Claims.First(x => x.Type == "CompanyId").Value);
            var id = await _mediator.Send(command);
            return Ok(new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int companyId = int.Parse(User.Claims.First(x => x.Type == "CompanyId").Value);
            var customers = await _mediator.Send(new GetCustomersQuery { CompanyId = companyId });
            return Ok(customers);
        }
    }
}
