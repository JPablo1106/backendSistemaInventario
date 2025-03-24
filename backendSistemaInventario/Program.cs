using backendSistemaInventario.Aplicacion.Usuarios;
using backendSistemaInventario.Aplicacion.Admin;
using backendSistemaInventario.Persistencia;
using backendSistemaInventario.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using backendSistemaInventario.Aplicacion.Equipos;
using backendSistemaInventario.Aplicacion.Componentes;


var builder = WebApplication.CreateBuilder(args);

//Configuracion del JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
    };
});

// Add services to the container.
builder.Services.AddControllers();

//Registro de JwtGenerator
builder.Services.AddScoped<JwtGenerator>();

//Servivios de Administrador
builder.Services.AddMediatR(typeof(RegistroAdministrador.Manejador).Assembly);
builder.Services.AddMediatR(typeof(RefreshTokenAdministrador.Manejador).Assembly);

//Servicios de usuarios
builder.Services.AddMediatR(typeof(RegistroUsuarios.Manejador).Assembly);
builder.Services.AddAutoMapper(typeof(ConsultaUsuarios.Manejador));
builder.Services.AddMediatR(typeof(ActualizarUsuario.Manejador).Assembly);
builder.Services.AddMediatR(typeof(EliminarUsuario.Manejador));

//Servicios de equipos
builder.Services.AddMediatR(typeof(RegistroEquipos.Manejador).Assembly);
builder.Services.AddAutoMapper(typeof(ConsultarEquipos.Manejador));
builder.Services.AddMediatR(typeof(ActualizarEquipo.Manejador));
builder.Services.AddMediatR(typeof(EliminarEquipo.Manejador));

//Servicios de componentes
builder.Services.AddMediatR(typeof(RegistroComponente.Manejador).Assembly);
builder.Services.AddMediatR(typeof(ActualizarComponente.Manejador));

//Conexion a SQL Server
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});

// Configuración de Swagger con soporte para JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de backendSistemaInventario", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese 'Bearer' [espacio] y luego su token JWT.\nEjemplo: Bearer 12345abcdef",
        Name = "Authorization",
        In = ParameterLocation.Header,
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
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

//Habilitar CORS antes de autenticacion
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

//Map controller y aplicar autorizacion global
app.MapControllers().RequireAuthorization();

app.Run();
