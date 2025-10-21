using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Proyecto2024.Client;
using Proyecto2024.Client.Autorizacion;
using Proyecto2024.Client.Servicios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScope(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//se cambia por Singleton para que sea una sola instancia en toda la aplicacion es decir que siempre sea el mismo objeto
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IHttpServicio, HttpServicio>();
//""
builder.Services.AddAuthorizationCore();

//cuando arranque el programa se va a crear una instancia de ProveedorAutenticacion
builder.Services.AddScoped<ProveedorAutenticacionJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacionJWT>( proveedor => 
proveedor.GetRequiredService<ProveedorAutenticacionJWT>());

//builder.Services.AddScoped< ILoginService, ProveedorAutenticacionJwt>();
builder.Services.AddScoped<ILoginService, ProveedorAutenticacionJWT>(proveedor =>
proveedor.GetRequiredService<ProveedorAutenticacionJWT>());

await builder.Build().RunAsync();
