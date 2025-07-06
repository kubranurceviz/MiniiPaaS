using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniiPaaS.Application.Commands.Auth;
using MiniiPaaS.Application.Commands.UserManagement;
using MiniiPaaS.Domain.Enums;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // MiniiPaaS.API/Controllers/AuthController.cs
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        // SuperAdmin sadece el ile oluşturulabilir (seed data)
        if (command.Role == Role.SuperAdmin)
            return BadRequest("SuperAdmin can only be created manually.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}