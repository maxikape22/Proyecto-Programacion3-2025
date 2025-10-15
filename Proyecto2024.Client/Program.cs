using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Proyecto2024.Client;
using Proyecto2024.Client.Autorizacion;
using Proyecto2024.Client.Servicios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScope(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//se cambia por Singleton para que sea una sola instancia en toda la aplicacion
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IHttpServicio, HttpServicio>();
//""
builder.Services.AddAuthorizationCore();

//cuando arranque el programa se va a crear una instancia de ProveedorAutenticacion
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

await builder.Build().RunAsync();
