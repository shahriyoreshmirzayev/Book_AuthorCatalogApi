using Application;
using BookCatalogApi.CustomMiddlewares;
using Infrastructure;
using Microsoft.OpenApi.Models;

namespace BookCatalogApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.WebHost.UseUrls("http://+:80");
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Bearer Authentication with JWT Token",
                Type = SecuritySchemeType.Http
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {{
                new OpenApiSecurityScheme()
                {
                   Reference=new OpenApiReference()
                   {
                       Id="Bearer",
                       Type=ReferenceType.SecurityScheme
                   }
                },
                new List<string>()
            } });
        });





        builder.Services.AddMemoryCache(); 
        builder.Services.AddStackExchangeRedisCache(setupAction =>
        {
            setupAction.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
        });
        builder.Services.AddResponseCaching();
        builder.Services.AddOutputCache();
        builder.Services.AddControllers();


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseResponseCaching();
        app.UseOutputCache();
        app.UseETagger();
        app.MapControllers();
        app.Run();
    }
}