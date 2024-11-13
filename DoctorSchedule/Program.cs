using DoctorSchedule.Application.Messaging.Implementation;
using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Infrastructure.Persistence;
using DoctorSchedule.Infrastructure.RepositoriesImplementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DoctorSchedule.Infrastructure")));


builder.Services.AddScoped<IEventRepository,EventRepository>();
builder.Services.AddScoped<IMessageQueue, InMemoryMessageQueue>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
