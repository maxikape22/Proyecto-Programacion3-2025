using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Proyecto2024.BD.Data;
using Proyecto2024.BD.Data.Entity;
using Proyecto2024.Server.Repositorio;

namespace Proyecto2024.Server.Controllers
{
    [ApiController]
    [Route("api/TDocumentos")]
    public class TDocumentosControllers : ControllerBase
    {
        private readonly ITDocumentoRepositorio repositorio;
        private readonly IOutputCacheStore outputCacheStore;

        //
        //
        //"" creo una constante para el tag del cache
        private const string cacheKey = "tDocumentos";

        //"" agrego la inyecccion del cache IOutputCacheStore 
        public TDocumentosControllers(ITDocumentoRepositorio repositorio, IOutputCacheStore outputCacheStore)
        {

            this.repositorio = repositorio;
            this.outputCacheStore = outputCacheStore;
        }
        
        [HttpGet]    //api/TDocumentos
        //agrego la etuiqueta del cache , si esta activo no se usa el select
        [OutputCache(Tags = [cacheKey])]
        public async Task<ActionResult<List<TDocumento>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener un objeto de tipo de documento
        /// </summary>
        /// <param name="id">Id del objeto</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] //api/TDocumentos/2
        [OutputCache(Tags = [cacheKey])]
        public async Task<ActionResult<TDocumento>> Get(int id)
        {
            TDocumento? pepe = await repositorio.SelectById(id);
            if (pepe == null)
            {
                return NotFound();
            }
            return pepe;
        }

        [HttpGet("GetByCod/{cod}")] //api/TDocumentos/GetByCod/DNI
        [OutputCache(Tags = [cacheKey])]
        public async Task<ActionResult<TDocumento>> GetByCod(string cod)
        {
            TDocumento? pepe = await repositorio.SelectByCod(cod);
            if (pepe == null)
            {
                return NotFound();
            }
            return pepe;
        }

        [HttpGet("existe/{id:int}")] //api/TDocumentos/existe/2
        [OutputCache(Tags = [cacheKey])]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(TDocumento entidad)
        {
            try
            {
                //return await repositorio.Insert(entidad);
                var id = await repositorio.Insert(entidad);
                if (id == 0)
                {
                    return BadRequest("No se pudo insertar el tipo de documento");
                }

                //""borrar el cache de forma asincrona
                await outputCacheStore.EvictByTagAsync(cacheKey, default);
                return id;
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")] //api/TDocumentos/2
        public async Task<ActionResult> Put(int id, [FromBody] TDocumento entidad)
        {
            try
            {
                if (id != entidad.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }
                var pepe = await repositorio.Update(id, entidad);

                if (!pepe)
                {
                    return BadRequest("No se pudo actualizar el tipo de documento");
                }
                //""borrar el cache de forma asincrona
                await outputCacheStore.EvictByTagAsync(cacheKey, default);

                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] //api/TDocumentos/2
        public async Task<ActionResult> Delete(int id)
        { 
            var resp = await repositorio.Delete(id);
            if (!resp)
            { return BadRequest("El tipo de documento no se pudo borrar"); }
            
            await outputCacheStore.EvictByTagAsync(cacheKey, default);

            return Ok();
        }

    }
}
