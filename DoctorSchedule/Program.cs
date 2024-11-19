using DoctorSchedule.Application.Messaging.Implementation;
using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Application.Services.Implementation;
using DoctorSchedule.Application.Services.Interface;
using DoctorSchedule.Domain.Configuration;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Infrastructure.Persistence;
using DoctorSchedule.Infrastructure.RepositoriesImplementation;
using DoctorSchedule.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using AutoMapper;
using DoctorSchedule.Application.CommandHandlers.Implementation;
using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Application.Security.Implementation;
using DoctorSchedule.Application.Security.Interface;
using DoctorSchedule.Application.QueryHandlers.Implementation;
using DoctorSchedule.Application.QueryHandlers.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Doctorly API App", Version = "v1" });

    // Add JWT Bearer Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Configure EF Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DoctorSchedule.Infrastructure")));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IEventRepository,EventRepository>();
builder.Services.AddScoped<IMessageQueue, InMemoryMessageQueue>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICreateAttendeeCommandHandler, CreateAttendeeCommandHandler>();
builder.Services.AddScoped<ICreateEventCommandHandler, CreateEventCommandHandler>();
builder.Services.AddScoped<IUpdateAttendeeCommandHandler, UpdateAttendeeCommandHandler>();
builder.Services.AddScoped<IApplicationDataProtector, ApplicationDataProtector>();
builder.Services.AddScoped<IGetAttendeeQueryHandler, GetAttendeeQueryHandler>();
builder.Services.AddScoped<IRemoveAttendeeCommandHandler, RemoveAttendeeCommandHandler>();
builder.Services.AddScoped<IAcceptEventCommandHandler, AcceptEventCommandHandler>();
builder.Services.AddScoped<IDeclineEventCommandHandler, DeclineEventCommandHandler>();
builder.Services.AddScoped<IGetEventByIdQueryHandler, GetEventByIdQueryHandler>();
builder.Services.AddScoped<IGetEventsBetweenDatesQueryHandler, GetEventsBetweenDatesQueryHandler>();
builder.Services.AddScoped<IUpdateEventCommandHandler, UpdateEventCommandHandler>();
builder.Services.AddScoped<IRemoveEventCommandHandler, RemoveEventCommandHandler>();

builder.Services.AddDataProtection();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
       .AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
