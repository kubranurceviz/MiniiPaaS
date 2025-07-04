﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniiPaaS.Application.Commands.Auth;
using MiniiPaaS.Application.Commands.Responses;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Domain.Entities;

namespace MiniiPaaS.Application.Commands.UserManagement;

public sealed class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LoginHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException("Kullanıcı bulunamadı.");

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Geçersiz giriş.");

        var token = _jwtService.GenerateToken(user);

        return new AuthResponse(
            Email: user.Email,
            Token: token,
            Role: user.Role,
            CompanyId: user.CompanyId
        );
    }
}
