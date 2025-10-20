using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto2024.Client.Servicios
{
    public static class IJSRuntimeExtensionMethods
    {
        //Llamo a la función JavaScript nativa confirm("texto") que muestra un cuadro de diálogo con “Aceptar” / “Cancelar”.
        //Devuelve true si el usuario acepta, false si cancela.
        public static async ValueTask<bool> Confirmar(this IJSRuntime js, string mensaje)
        {
            return await js.InvokeAsync<bool>("confirm", mensaje);
        }

        //Llamo a localStorage.setItem(key, value) en el navegador.
        //Guarda datos en el almacenamiento local del navegador.
        public static ValueTask<object> GuardarEnLocalStorage(this IJSRuntime js,
            string llave, string contenido)
        {
            return js.InvokeAsync<object>("localStorage.setItem", llave, contenido);
        }

        //Recupero un valor guardado en localStorage con la clave dada.
        //Devuelvo un object (puedo convertirlo o deserializarlo).
        public static ValueTask<object> ObtenerDeLocalStorage(this IJSRuntime js,
            string llave)
        {
            return js.InvokeAsync<object>("localStorage.getItem", llave);
        }

        //Elimino una entrada del localStorage del navegador.
        //Equivale a localStorage.removeItem(key) en JavaScript.
        public static ValueTask<object> RemoverDelLocalStorage(this IJSRuntime js,
            string llave)
        {
            return js.InvokeAsync<object>("localStorage.removeItem", llave);
        }
    }
}