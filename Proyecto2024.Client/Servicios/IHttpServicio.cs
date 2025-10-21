
namespace Proyecto2024.Client.Servicios
{
    public interface IHttpServicio
    {
        Task<HttpRespuesta<T>> Get<T>(string url);
        Task<HttpRespuesta<TipoRespuesta>> Post<T, TipoRespuesta>(string url, T entidad);
        Task<HttpRespuesta<object>> Put<T>(string url, T entidad);
        Task<HttpRespuesta<object>> Delete(string url);
    }
}