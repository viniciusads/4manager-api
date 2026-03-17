using _4Tech._4Manager.API.Middlewares;
using _4Tech._4Manager.API.Options;
using _4Tech._4Manager.Application.Common.Behaviors;
using _4Tech._4Manager.Application.Common.Pagination;
using _4Tech._4Manager.Application.DependencyInjection;
using _4Tech._4Manager.Application.Features.Projects.Validators;
using _4Tech._4Manager.Application.Features.Timesheets.Validators;
using _4Tech._4Manager.Application.Mapping;
using _4Tech._4Manager.Infrastructure.Config.DependencyInjection;
using _4Tech._4Manager.Persistence.Config.DependencyInjection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Supabase;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHttpClient("PdfService", client =>
{
    client.BaseAddress = new Uri(configuration["PdfService:BaseUrl"]);
});

// =================================================================================
// 1. CARREGAMENTO E VALIDAÇÃO DE CONFIGURAÇÕES (APPSETTINGS)
// =================================================================================

// Variáveis que serão preenchidas pelo JSON
string supabaseUrl = "";
string supabaseKey = "";
string supabaseServiceKey = "";
string jwtSecret = "";
string validIssuer = "";
string validAudience = "";
string connectionString = "";
string[] allowedOrigins = Array.Empty<string>();

try
{
    Console.WriteLine("--> Lendo configurações do appsettings.json...");

    // Função auxiliar para validar se a chave existe no JSON
    string GetConfig(string key)
    {
        var value = configuration[key];
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"[ERRO DE CONFIGURAÇÃO] A chave '{key}' é obrigatória e não foi encontrada no appsettings.json.");
        return value;
    }

    // Leitura das configurações
    connectionString = configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("[ERRO] ConnectionString 'DefaultConnection' não encontrada.");

    supabaseUrl = GetConfig("Supabase:Url");
    supabaseKey = GetConfig("Supabase:Key");
    supabaseServiceKey = GetConfig("Supabase:ServiceRoleKey");

    jwtSecret = GetConfig("Jwt:Secret");
    validIssuer = GetConfig("Jwt:Issuer");
    validAudience = GetConfig("Jwt:Audience");

    // Leitura do CORS (Array de Strings)
    allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>() 
                     ?? new[] { "http://localhost:4200" };

    Console.WriteLine("--> Configurações carregadas com sucesso.");
}
catch (Exception ex)
{
    // Caso não tenha preenchido alguma configuração obrigatória, exibe o erro e encerra a aplicação
    Console.Error.WriteLine($"\n\n[FATAL] FALHA NA INICIALIZAÇÃO:\n{ex.Message}\n\n");
    Environment.Exit(1);
}

// =================================================================================
// 2. CONFIGURAÇÃO DE SERVIÇOS
// =================================================================================

var options = new SupabaseOptions { AutoRefreshToken = true, AutoConnectRealtime = true };

// Injeta o Client do Supabase com as configs lidas
builder.Services.AddSingleton(provider => new Supabase.Client(supabaseUrl, supabaseServiceKey, options));

// Validadores do FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<GetProjectByIdQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetProjectsByStatusQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetTicketsByProjectIdQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetTicketDetailsByTicketIdQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTicketNoteCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTicketCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetTimesheetByPeriodQueryValidator>();

// Configuração do MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddHealthChecks();

// Configurações de senha (Identity)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
});

builder.Services.Configure<PaginationOptions>(
    builder.Configuration.GetSection("Pagination"));

builder.Services.PostConfigure<PaginationOptions>(opts =>
{
    if (opts.DefaultPageNumber <= 0) opts.DefaultPageNumber = 1;
    if (opts.DefaultPageSize <= 0) opts.DefaultPageSize = 20;
    if (opts.MaxPageSize <= 0)
        opts.MaxPageSize = PaginationDefaults.MaxPageSize;
    if (opts.DefaultPageSize > opts.MaxPageSize) opts.DefaultPageSize = opts.MaxPageSize;
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

// Políticas de Autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GestorOnly", policy => policy.RequireRole("Gestor"));

    options.AddPolicy("ClienteOnly", policy =>
     policy.RequireAssertion(context =>
        context.User.IsInRole("Cliente") || context.User.IsInRole("Gestor")
    ));

    options.AddPolicy("FuncionarioOnly", policy =>
    policy.RequireAssertion(context =>
        context.User.IsInRole("Funcionario") || context.User.IsInRole("Gestor")
    ));

    options.AddPolicy("CanEdit", policy =>
     policy.RequireClaim("accessLevel", "Editar", "AdministradorTotal"));

    options.AddPolicy("CanView", policy =>
    policy.RequireClaim("accessLevel", "Visualizar", "Editar", "AdministradorTotal"));
});

// Configuração de Autenticação JWT
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var key = Encoding.UTF8.GetBytes(jwtSecret);
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = validIssuer,
            ValidateAudience = true,
            ValidAudience = validAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
        
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Append("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    });

// Configuração de CORS (lendo do appsettings)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins(allowedOrigins) // Usa a lista do appsettings
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// AutoMapper e Camadas da Clean Architecture
builder.Services.AddAutoMapper(typeof(UserProfileMappingProfile), typeof(ProjectProfile), typeof(TicketProfile), typeof(CustomerProfile), typeof(TimesheetProfile));
builder.Services.AddApplication();
builder.Services.AddPersistence(connectionString);
builder.Services.AddInfrastructure();

// =================================================================================
// 3. PIPELINE DE EXECUÇÃO
// =================================================================================

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngular");
app.UseRouting();

// Middleware global de tratamento de exceções
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Console.WriteLine("--> Aplicação iniciada com sucesso.");

app.Run();