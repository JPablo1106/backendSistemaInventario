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
using backendSistemaInventario.Aplicacion.EquiposSeguridad;
using System.Text.Json;
using System.Text.Json.Serialization;


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
builder.Services.AddAutoMapper(typeof(ComponentesProfile));
builder.Services.AddMediatR(typeof(ActualizarComponente.Manejador));

//Servicios de equipos de seguridad
builder.Services.AddMediatR(typeof(RegistroEquipoSeguridad));
builder.Services.AddAutoMapper(typeof(EquiposSeguridadProfile));

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

// Middleware global de manejo de excepciones.
// Este middleware captura cualquier excepción que se lance en la cadena de ejecución y
// asegura que se genere una respuesta en formato JSON con el código 500.
// De esta forma, se evita que se interrumpa el pipeline y se pierda la inyección de los headers CORS.
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        // Aquí puedes registrar la excepción según tu sistema de logging.
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var errorResponse = new { error = ex.Message };
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
});

// Habilitar CORS antes de la autenticación y autorización
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Se mapean los controladores y se requiere autorización globalmente
app.MapControllers().RequireAuthorization();

app.Run();
