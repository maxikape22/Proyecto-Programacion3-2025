using Microsoft.AspNetCore.Components.Authorization;
using Proyecto2024.Client.Servicios;
using System.Security.Claims;

namespace Proyecto2024.Client.Autorizacion
{
    public class ProveedorAutenticacion : AuthenticationStateProvider
    {
        //la clase la ponemos como clase abstacta que nos permite sobreescribir
        //el metodo GetAuthenticationStateAsync() que pertenece a la clase heredada
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //""
            await Task.Delay(5000);//simulo una espera de 5 segundos
            //simulamos un usuario anonimo
            var anonimo = new ClaimsIdentity();

            var usuariopepe = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name,"pepe"),
                    new Claim(ClaimTypes.Role,"admin"),
                    new Claim("DNI","15.690.876")

                }, 
                authenticationType: "ok"
                );
            var usuariojuancito = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name,"Juancito"),
                    new Claim(ClaimTypes.Role,"operador"),
                    new Claim("dni","14579876")

                }, 
                authenticationType: "ok"
                );
            //return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonimo)));//usuario anonimo
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(usuariopepe)));//usuario anonimo


        }
    }
}
