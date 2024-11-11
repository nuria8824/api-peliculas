using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// Cargar las variables de entorno desde el archivo .env
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores para manejar las solicitudes HTTP
builder.Services.AddControllers();

// Configurar la cadena de conexión para la base de datos, reemplazando las variables de entorno en la conexión
var connetionString = builder.Configuration.GetConnectionString("cnPeliculas");
connetionString = connetionString.Replace("SERVER_NAME", builder.Configuration["SERVER_NAME"]);
connetionString = connetionString.Replace("DB_USER", builder.Configuration["DB_USER"]);
connetionString = connetionString.Replace("DB_PASS", builder.Configuration["DB_PASS"]);

// Configurar Swagger para la documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Información básica de la API en Swagger
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tu API", Version = "v1" });

    // Configuración de seguridad para JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT en este formato: Bearer {token}"
    });

    // Requisitos de seguridad para usar JWT en la API
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

// Añadir el contexto de base de datos de la aplicación con SQL Server
builder.Services.AddSqlServer<PeliculasContext>(connetionString);

// Registrar los servicios para manejar las películas y críticas en la base de datos
builder.Services.AddScoped<IPeliculaService, PeliculaDbService>();
builder.Services.AddScoped<ICriticaService, CriticaDbService>();

// Configurar el contexto de base de datos para Identity (usado para autenticación y autorización)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connetionString));

// Configurar Identity, que maneja los usuarios y roles para la autenticación
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configurar JWT para autenticación
builder.Services.AddAuthentication(options =>
{
    // Configurar el esquema de autenticación como JWT
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Parámetros de validación del token JWT
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Valida que el emisor del token sea el esperado
        ValidateAudience = true, // Valida que la audiencia del token sea la esperada
        ValidateLifetime = true, // Valida que el token no haya expirado
        ValidateIssuerSigningKey = true, // Verifica que el token esté firmado con la clave correcta
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Emisor esperado del token
        ValidAudience = builder.Configuration["Jwt:Audience"], // Audiencia esperada del token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Clave secreta para firmar el token
    };
});

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    // Activar Swagger solo en desarrollo para documentar la API
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirigir las solicitudes HTTP a HTTPS
app.UseHttpsRedirection();

// Habilitar el uso de controladores para manejar las solicitudes
app.MapControllers();

// Iniciar la aplicación
app.Run();
