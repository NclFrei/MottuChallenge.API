using Microsoft.EntityFrameworkCore;
using MottuChallenge.Application.Service;
using MottuChallenge.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura o DbContext com a string do Oracle
builder.Services.AddDbContext<MottuChallengeContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// 2. Registra o seu servi�o de usu�rio para inje��o de depend�ncia
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PatioService>();
// Se voc� tiver uma interface IUserService, prefira:
// builder.Services.AddScoped<IUserService, UserService>();

// 3. Configura controllers, Swagger e OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Pipeline de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();