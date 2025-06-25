using MediatR;
using MiniiPaaS.Application.Commands.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniiPaaS.Application.Commands.Auth
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;
}
