using Dsw2025Tpi.Api.Configurations;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Dsw2025Tpi.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDomainServices(builder.Configuration);

        //builder.Services.AddSingleton<IRepository, InMemoryRepository>();

        builder.Services.AddSwaggerGen();


        builder.Services.AddHealthChecks();

        builder.Services.AddDomainServices(builder.Configuration);


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<Dsw2025Tpi.Api.Middlewares.ExceptionHandlingMiddleware>();


        app.UseAuthorization();

        app.MapControllers();
        
        app.MapHealthChecks("/healthcheck");
        app.Run();
    }
}
