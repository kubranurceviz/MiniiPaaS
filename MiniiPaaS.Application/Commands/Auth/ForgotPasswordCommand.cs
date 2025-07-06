// MiniiPaaS.Application/Commands/Auth/ForgotPasswordCommand.cs
using MediatR;

namespace MiniiPaaS.Application.Commands.Auth
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }
}

