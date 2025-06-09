using Microsoft.EntityFrameworkCore;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger com suporte a JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token_aqui}"
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
            new string[] {}
        }
    });
});

// Serviços do projeto
builder.Services.AddScoped<IRelatorioService, RelatorioService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProjetoService, ProjetoService>();
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<IUtilizadorService, UtilizadorService>();
builder.Services.AddScoped<IConviteService, ConviteService>();
builder.Services.AddScoped<IRelatorioProjetoService, RelatorioProjetoService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// DB Context
builder.Services.AddDbContext<GestaoServicosClientesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuting))
);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            NameClaimType = ClaimTypes.NameIdentifier // ✅ ESSENCIAL para FindFirst funcionar
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // ✅ OBRIGATÓRIO antes de UseAuthorization
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// ⚙️ Criação do admin default
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GestaoServicosClientesContext>();
    context.Database.Migrate();

    var admin = context.Utilizadores.FirstOrDefault(u => u.Email == "admin@admin.com");

    if (admin == null)
    {
        admin = new Utilizador
        {
            Nome = "Admin",
            Email = "admin@admin.com",
            HorasDia = 8,
            Tipo = "Admin",
            IsAdmin = true
        };

        var hasher = new PasswordHasher<Utilizador>();
        admin.Password = hasher.HashPassword(admin, "admin123");

        context.Utilizadores.Add(admin);
        context.SaveChanges();
    }
    else
    {
        var hasher = new PasswordHasher<Utilizador>();
        var result = hasher.VerifyHashedPassword(admin, admin.Password, "admin123");

        if (result == PasswordVerificationResult.Failed)
        {
            admin.Password = hasher.HashPassword(admin, "admin123");
        }

        admin.Tipo = "Admin";
        admin.IsAdmin = true;
        context.SaveChanges();
    }

    // ✅ (Opcional) Mostrar claims do admin no arranque
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
        new Claim(ClaimTypes.Role, admin.Tipo),
    };
}

app.Run();
