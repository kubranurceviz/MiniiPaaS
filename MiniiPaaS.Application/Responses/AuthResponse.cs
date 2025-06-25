using MiniiPaaS.Domain.Enums;

namespace MiniiPaaS.Application.Commands.Responses
{
    public record AuthResponse(
        string Email,
        string Token,
        Role Role,
        int CompanyId
    );
}
