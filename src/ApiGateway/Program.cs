using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Configure JWT authentication
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings["Issuer"],
//            ValidAudience = jwtSettings["Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
//            )
//        };
//    });

//builder.Services.AddAuthorization(options =>
//    {
//        options.AddPolicy("default", policy =>
//        {
//            policy.RequireAuthenticatedUser();
//        });
//    });

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapReverseProxy();

app.Run();