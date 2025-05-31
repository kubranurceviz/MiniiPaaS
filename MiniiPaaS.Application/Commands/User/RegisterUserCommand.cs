using Docker.DotNet.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniiPaaS.Application.Commands.User
{
    public record RegisterUserCommand(string Email, string Password) : IRequest<AuthResponse>;
    
   
}
