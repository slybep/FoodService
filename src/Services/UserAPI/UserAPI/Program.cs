using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using UserAPI;
using UserAPI.Abstractions;
using UserAPI.Repository;
using UserAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication("CustomJwt")
    .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>("CustomJwt", null);
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins(
                "http://localhost:7293",  // AuthorizationAPI (если фронтенд на нём)
                "http://localhost:3000"   // адрес вашего фронтенд-приложения (React, etc)
              )
              .AllowCredentials()         // Разрешает передачу кук
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DevCors");
app.UseMiddleware<JwtServiceExtenshions>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
