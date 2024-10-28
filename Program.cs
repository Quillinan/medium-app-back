using medium_app_back.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using medium_app_back.Repositories;
using medium_app_back.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = $"Server={Env.GetString("DATABASE_SERVER")};Database={Env.GetString("DATABASE_NAME")};Integrated Security=True;TrustServerCertificate=True;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<PostService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
