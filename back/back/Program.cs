using Microsoft.EntityFrameworkCore;
using back.Models;

var builder = WebApplication.CreateBuilder(args);

// Register controllers and other services
builder.Services.AddControllers();

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register your DbContext for PostgreSQL
builder.Services.AddDbContext<GestaoServicosClientesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register a CORS policy that allows any origin, header, and method
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Enable Swagger middleware in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply the CORS policy before other middleware like authorization
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();
app.Run();
