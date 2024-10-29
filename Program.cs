using medium_app_back.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using medium_app_back.Repositories;
using medium_app_back.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(provider =>
{
    var encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY");

    if (string.IsNullOrEmpty(encryptionKey))
    {
        throw new ArgumentException("A chave de criptografia não pode ser nula ou vazia.");
    }

    return encryptionKey;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:5173", "https://seu-dominio-deploy.com")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

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

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
