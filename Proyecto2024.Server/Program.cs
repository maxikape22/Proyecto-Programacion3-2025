using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecto2024.BD.Data;
using Proyecto2024.BD.Usuario;
using Proyecto2024.Server.Repositorio;
using System.Text.Json.Serialization;

//------------------------------------------------------------------
//configuracion de los servicios en el constructor de la aplicación
var builder = WebApplication.CreateBuilder(args);

//
//
//""aca creo el constructor del cache

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
});

//"" reemplazo el cache anterior por este que usa redis
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("redis");
//});

builder.Services.AddControllers().AddJsonOptions(
    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(op => op.UseSqlServer("name=conn"));

//""agrego los servicios de identidad
builder.Services.AddIdentity<MiUsuario, IdentityRole>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

//""agrego el servicio de autenticacion con jwt defino las variables del encabezado
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

//.AddJwtBearer(options =>

//{

//    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters

//    {
//        //validando el origen del token
//        ValidateIssuer = false,

//        ValidateAudience = false,

//        ValidateLifetime = true,

//        ValidateIssuerSigningKey = true,

//        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(

//    System.Text.Encoding.UTF8.GetBytes(builder.Configuration["jwtkey"])),

//        ClockSkew = TimeSpan.Zero // Reducir el sesgo del reloj a cero para fines de prueba

//    };

//});

//""agrego el servicio de autenticacion con jwt defino las variables del encabezado
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["jwtkey"]))
        };
    });

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ITituloRepositorio, TituloRepositorio>();
builder.Services.AddScoped<ITDocumentoRepositorio, TDocumentoRepositorio>();

//--------------------------------------------------------------------
//construccón de la aplicación
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

//""
app.UseAuthentication();
//""
app.UseAuthorization();

//
//
//agrego el use cache
//
app.UseOutputCache();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
