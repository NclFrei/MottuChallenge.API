using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using MottuChallenge.API.Application.Mapper;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Validator;
using MottuChallenge.API.Infrastructure.Configuration;
using MottuChallenge.API.Infrastructure.Data;
using MottuChallenge.API.Infrastructure.Repository;
using MottuChallenge.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ✅ DATABASE
builder.Services.AddDbContext<MottuChallengeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ JWT SETTINGS
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWTSettings"));
var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JwtSettings>();

// ✅ AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(UserProfile).Assembly);
    cfg.AddMaps(typeof(MotoProfile).Assembly);
    cfg.AddMaps(typeof(PatioProfile).Assembly);
});

// ✅ Services + Repositories
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

// ✅ Validators
builder.Services.AddScoped<IValidator<UserCreateRequest>, UserCreateRequestValidator>();
builder.Services.AddScoped<IValidator<AreaRequest>, AreaCreateRequestValidator>();
builder.Services.AddScoped<IValidator<PatioRequest>, PatioCreateRequestValidator>();
builder.Services.AddScoped<IValidator<EnderecoRequest>, EnderecoCreateRequestValidator>();
builder.Services.AddScoped<IValidator<MotoRequest>, MotoCreateRequestValidator>();

// ✅ JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// ✅ API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version")
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ✅ Swagger com Versionamento
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MottuChallenge API", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "MottuChallenge API", Version = "v2" });

    // JWT no Swagger
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Insira o token JWT válido abaixo",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddControllers();

// ✅ Health Check
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDbContextCheck<MottuChallengeContext>("database");


// ✅ BUILD APP
var app = builder.Build();

// ✅ Middleware de Erros
app.UseMiddleware<ExceptionMiddleware>();

// ✅ Swagger UI com versões
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MottuChallenge API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "MottuChallenge API V2");
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// ✅ Endpoints Health Check
app.MapHealthChecks("/health")
   .WithTags("Health");

app.MapHealthChecks("/health/details", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            results = report.Entries.Select(e => new
            {
                component = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
}).WithTags("Health");

app.MapControllers();

app.Run();
