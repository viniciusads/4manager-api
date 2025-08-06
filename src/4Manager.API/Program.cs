using _4Manager.Application.DependencyInjection;
using _4Manager.Infrastructure.Config.DependencyInjection;
using _4Manager.Persistence.Config.DependencyInjection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Supabase;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var url = builder.Configuration["Supabase:ProjectUrl"]!;
var key = builder.Configuration["Authentication:SupabaseApiKey"]!;
var serviceRoleKey = builder.Configuration["Authentication:ServiceRoleKey"]!;

var options = new SupabaseOptions { AutoRefreshToken = true, AutoConnectRealtime = true };

builder.Services.AddSingleton(provider => new Supabase.Client(url, serviceRoleKey, options));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddControllers();

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
                     new string[] { }
                 }
             });
}); 


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("AnalistaOnly", policy => policy.RequireRole("Analista"));
});

var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtSecret"]!);

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters { 
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(bytes),
    ValidAudience = builder.Configuration["Auth:ValidAudience"],
    ValidIssuer = builder.Configuration["Auth:ValidIssuer"],
    };
});


builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();