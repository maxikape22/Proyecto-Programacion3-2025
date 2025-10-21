using Proyecto2024.Shared.DTO;

namespace Proyecto2024.Client.Autorizacion
{
    public interface ILoginService
    {
        Task Login(UserTokenDTO tokenDTO);
        Task Logout();
    }
}