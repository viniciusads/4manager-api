using _4Manager.Persistence.Context;
using _4Tech._4Manager.API.Middlewares;
using _4Tech._4Manager.Application.DependencyInjection;
using _4Tech._4Manager.Application.Mapping;
using _4Tech._4Manager.Infrastructure.Config.DependencyInjection;
using _4Tech._4Manager.Persistence.Config.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Supabase;
using System.Text;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

static string GetEnv(string name)
{
    return DotNetEnv.Env.GetString(name)
           ?? throw new InvalidOperationException($"{name} não está configurado.");
}

var url = GetEnv("SUPABASE_PROJECT_URL");
var apikey = GetEnv("SUPABASE_API_KEY");
var serviceRoleKey = GetEnv("SUPABASE_SERVICE_ROLE_KEY");
var jwtSecret = GetEnv("JWT_SECRET");
var validIssuer = GetEnv("AUTH_VALID_ISSUER");
var validAudience = GetEnv("AUTH_VALID_AUDIENCE");
var connectionString = GetEnv("DB_CONNECTION");

var options = new SupabaseOptions { AutoRefreshToken = true, AutoConnectRealtime = true };

builder.Services.AddSingleton(provider => new Supabase.Client(url, serviceRoleKey, options));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "4Manager API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira o token JWT no campo. Ex: Bearer {seu_token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                     Array.Empty<string>()
                 }
             });
}); 

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("AnalistaOnly", policy => policy.RequireRole("Analista"));
});

var keyBytes = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters { 
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
    ValidAudience = validAudience,
    ValidIssuer = validIssuer,
    };
});

builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddApplication();
builder.Services.AddPersistence(connectionString);
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapHealthChecks("/health");
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();