using Microsoft.AspNetCore.Mvc;
using UsersAPI.Domain.Interfaces.Service;

namespace UsersAPI.Application.Endpoint;

public static class Login
{
    public static void MapLoginEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (UserLoginDto login, IUserService userService,
            [FromServices] IPasswordService passwordService, [FromServices] IJwtService jwtService) =>
        {
            var user = await userService.GetUserByEmail(login.Email);
            if (user is null || !passwordService.VerifyPassword(user, login.Password))
                return Results.Unauthorized();
            var token = jwtService.GenerateToken(user);
            return Results.Ok(new { token });
        }).WithTags("Login");
    }
}

public class UserLoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}