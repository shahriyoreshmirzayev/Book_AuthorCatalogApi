﻿using Application.Abstraction;
using Application.Repositories;
using Application.Services;
using Infrastructure.DataAccess;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure;

public static class RegisterServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IBookCatalogDbContext, BookCatalogDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("BookCatalogConnection")));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        //services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.SaveToken = true;
                            options.TokenValidationParameters = new()
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidAudience = configuration["JWT:AudienceKey"],
                                ValidIssuer = configuration["JWT:IssuerKey"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                                ClockSkew = TimeSpan.Zero
                            };
                            options.Events = new JwtBearerEvents
                            {
                                OnAuthenticationFailed = (context) =>
                                {
                                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                    {
                                        context.Response.Headers.Add("IsTokenExpired", "true");
                                    }
                                    return Task.CompletedTask;
                                }
                            };
                        });
        return services;
    }
}