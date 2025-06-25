using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MiniiPaaS.Domain.Entities; // Tam namespace path

namespace MiniiPaaS.Application.Commands.UserManagement;


public record CreateUserCommand(string Email, string Password) : IRequest<MiniiPaaS.Domain.Entities.User>;