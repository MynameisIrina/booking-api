using BookingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using FastEndpoints;
using MediatR;
using BookingApi.Application.Interfaces.Behaviors;
using BookingApi.Application.Repositories;
using BookingApi.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(SaveChangesBehavior<,>));

// Connect to database
builder.Services.AddDbContext<BookingDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IRoomSlotRepository, RoomSlotRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
    Console.WriteLine("Applying migrations...");
    dbContext.Database.Migrate();
    Console.WriteLine("Migrations applied successfully.");
}

app.Run();
