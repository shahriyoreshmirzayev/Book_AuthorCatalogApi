using Infrastructure;
using Application;
using BookCatalogApi.CustomMiddlewares;

namespace BookCatalogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
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
}