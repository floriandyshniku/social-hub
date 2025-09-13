using newProject.Application.Users.Commands.CreateUser;
using newProject.Application.Users.Commands.UpdateUser;
using newProject.Application.Users.Commands.DeleteUser;
using newProject.Application.Users.Queries.GetUser;
using newProject.Application.Users.Queries.GetAllUsers;
using newProject.Domain.Users;
using newProject.Domain.Users.Services;
using newProject.Infrastructure.Data;
using newProject.Infrastructure.Repositories;
using newProject.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using newProject.Application.Users.Commands.FollowUser;
using MediatR;
using System.Reflection;
using FluentValidation;
using newProject.Application.Posts.Commands.CreatePost;
using newProject.Infrastructure.Middleware;
using FluentValidation.AspNetCore;
using newProject.Domain.Posts;
using newProject.Infrastructure.RealTime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR
builder.Services.AddSignalR();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register domain services
builder.Services.AddScoped<IUserDomainService, UserDomainService>();

// Register real-time services
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add exception handling middleware (early in pipeline)
app.UseExceptionHandling();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map SignalR hub
app.MapHub<NotificationHub>("/notificationHub");

// Add health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

app.Run();
