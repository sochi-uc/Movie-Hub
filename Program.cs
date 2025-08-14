using Microsoft.EntityFrameworkCore;
using Movie_Hub.Model;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext <MovieCollectionContext > (options =>
    options.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 33))));

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
app.MapControllers();

app.Run();
