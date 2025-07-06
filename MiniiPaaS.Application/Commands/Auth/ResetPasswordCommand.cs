// MiniiPaaS.Application/Commands/Auth/ResetPasswordCommand.cs
using MediatR;

namespace MiniiPaaS.Application.Commands.Auth
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}