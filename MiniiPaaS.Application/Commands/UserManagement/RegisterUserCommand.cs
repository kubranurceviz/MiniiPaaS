using MediatR;
using MiniiPaaS.Domain.Enums;

namespace MiniiPaaS.Application.Commands.UserManagement
{
    public record RegisterUserCommand(
        string Email,
        string Password,
        Role Role,
        int CompanyId
    ) : IRequest<MiniiPaaS.Application.Commands.Responses.AuthResponse>;
}
