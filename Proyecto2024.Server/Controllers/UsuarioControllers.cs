using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto2024.Shared.DTO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Proyecto2024.Server.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuarioControllers : ControllerBase
    {

        public UserManager<IdentityUser> userManager { get; }
        public SignInManager<IdentityUser> signInManager { get; }
        public IConfiguration configuration { get; }
        // el UserManager es una clase oservicio que me permite manejar los usuarios
        //SignInManager es una clase que maneja las firmas(claves) para para autorizacion y aut del usuario
        public UsuarioControllers(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] UserInfoDTO userInfoDTO)
        {
            var resultado = await signInManager.PasswordSignInAsync(userInfoDTO.Email, userInfoDTO.Password, isPersistent: false, lockoutOnFailure: false);
           
            if (resultado.Succeeded)
            {
                return await ConstruirToken(userInfoDTO);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }


        [HttpPost("registrar")]
        public async Task<ActionResult<UserTokenDTO>> RegistrarUsuario([FromBody] UserInfoDTO userInfoDTO)
        {
            var usuario = new IdentityUser { UserName = userInfoDTO.Email, Email = userInfoDTO.Email };
            var resultado = await userManager.CreateAsync(usuario, userInfoDTO.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(userInfoDTO);
            }
            else
            {
                return BadRequest(resultado.Errors.First());
            }

        }
        // metodo construir el jwt
        private async Task<UserTokenDTO> ConstruirToken(UserInfoDTO userInfoDTO)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,userInfoDTO.Email),
                new Claim(ClaimTypes.Email,userInfoDTO.Email),
                new Claim("miValor","Lo que yo quiera")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]!));
            //creamos las credenciales
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // a la fecha actual UtcNow le adiciono 1 año
            var expiration = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken (                
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credenciales
                );

            return new UserTokenDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiration
            };
        }


    }
}
