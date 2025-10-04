using Amazon;
using Amazon.DynamoDBv2;
using AutoMapper;
using LoansPlatform.Application.Mappings;
using LoansPlatform.Application.Services;
using LoansPlatform.Domain.Interfaces;
using LoansPlatform.Infrastructure.Cache;
using LoansPlatform.Infrastructure.Persistence;
using LoansPlatform.Infrastructure.Repositories;
using LoansPlatform.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);



// PostgreSQL - EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));



// Services (Application)
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LoanService>();

// AWS DynamoDB client
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var config = builder.Configuration.GetSection("AWS");
    return new AmazonDynamoDBClient(
        config["AccessKey"],
        config["SecretKey"],
        RegionEndpoint.GetBySystemName(config["Region"])
    );
});

// Cache service
builder.Services.AddScoped<ILoanCache, DynamoDbLoanCache>();
builder.Services.AddScoped<IUserCache, DynamoDbUserCache>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();



var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loans Platform API v1");
       
    });
}


app.UseHttpsRedirection();


app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
