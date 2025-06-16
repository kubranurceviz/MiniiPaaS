using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Domain.Entities;

namespace MiniiPaaS.Application.Commands.User;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IJwtService _jwtService;
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher<MiniiPaaS.Domain.Entities.User> _passwordHasher;

    public RegisterUserHandler(
        IJwtService jwtService,
        IApplicationDbContext context,
        IPasswordHasher<MiniiPaaS.Domain.Entities.User> passwordHasher)
    {
        _jwtService = jwtService;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (existing is not null)
            throw new Exception("User already exists.");

        var user = new MiniiPaaS.Domain.Entities.User { Email = request.Email };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtService.GenerateToken(user);

        return new AuthResponse { Token = token, Email = user.Email };
    }
}