using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using FluentValidation;
using MottuChallenge.API.Application.Mapper;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Validator;
using MottuChallenge.API.Infrastructure.Configuration;
using MottuChallenge.API.Infrastructure.Data;
using MottuChallenge.API.Infrastructure.Repository;
using MottuChallenge.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MottuChallengeContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));



builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWTSettings"));
var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JwtSettings>();


builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(UserProfile).Assembly);
    cfg.AddMaps(typeof(MotoProfile).Assembly);
    cfg.AddMaps(typeof(PatioProfile).Assembly);
    cfg.AddMaps(typeof(UserProfile).Assembly);
    
});



builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PatioService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AreaService>();
builder.Services.AddScoped<MotoService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IJwtSettingsProvider, JwtSettingsProvider>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<IPatioRepository, PatioRepository>();

builder.Services.AddScoped<IValidator<UserCreateRequest>, UserCreateRequestValidator>();
builder.Services.AddScoped<IValidator<AreaRequest>, AreaCreateRequestValidator> ();
builder.Services.AddScoped<IValidator<PatioRequest>, PatioCreateRequestValidator>();
builder.Services.AddScoped<IValidator<EnderecoRequest>, EnderecoCreateRequestValidator>();
builder.Services.AddScoped<IValidator<MotoRequest>, MotoCreateRequestValidator>();


var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // ...
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Autentica��o falhou: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado com sucesso!");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Usuarios.API", Version = "v1" });

    var jwtSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Description = "Insira o token JWT no campo abaixo.",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
