using Microsoft.EntityFrameworkCore;
using UttuHub.API.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Connection Strings

// DbContext Registration/ Dependency Injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger ko UI visible garauna
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
} 

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

