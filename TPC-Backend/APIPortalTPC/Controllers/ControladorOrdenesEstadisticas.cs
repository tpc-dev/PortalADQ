using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
  

    public class ControladorOrdenesEstadisticas : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioOrdenesEstadisticas ROE;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="ROE">Interface de RepositorioOrdenesEstadisticas</param>

        public ControladorOrdenesEstadisticas(IRepositorioOrdenesEstadisticas ROE)
        {
            this.ROE = ROE;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Lista con todos los objetos Ordenes_Estadisticas</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await ROE.GetAllOE());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la orden estadistica: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id que se usa para buscar el Objeto Ordenes_Estadisticas</param>
        /// <returns>Retorna el objeto Ordenes_Estadisticas buscado</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await ROE.GetOE(id);
                if (resultado.Id_Orden_Estadistica == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro la orden estadistica");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la orden estadistica: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="OE">Objeto Ordenes_Estadistica que se añadirá a la base de datos</param>
        /// <returns>Retorna el objeto creado</returns>
        [HttpPost]
        public async Task<ActionResult<OrdenesEstadisticas>> Nuevo(OrdenesEstadisticas OE)
        {
            try
            {
                if (OE == null)
                    return BadRequest();

                OrdenesEstadisticas nuevo = await ROE.NuevoOE(OE);
                return nuevo;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        /// <summary>
        /// Metodo asincrónico para modificar un objeto por ID
        /// </summary>
        /// <param name="OE">Objeto Ordenes_Estadisticas que se usara para reemplazar su homonimo en la base de datos</param>
        /// <param name="id">Id del objeto Ordenes_Estadisticas que se quiere reemplazar</param>
        /// <returns>Retorna el objeto nuevo </returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrdenesEstadisticas>> Modificar(OrdenesEstadisticas OE, int id)
        {
            try
            {
                if (id != OE.Id_Orden_Estadistica)
                    return BadRequest("La Id no coincide");

                var Modificar = await ROE.GetOE(id);

                if (Modificar == null)
                    return NotFound($"Centro de Costo con = {id} no encontrado");

                return await ROE.ModificarOE(OE);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos " +ex.Message);
            }
        }
        /// <summary>
        /// Metodo para "eliminar" una Orden estadistica, asi no se puede utilizar y no se vera
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<OrdenesEstadisticas>> Eliminar(int id)
        {
            try
            {
                var u = ROE.GetOE(id);
                if (u == null)
                {
                    return NotFound("No se encontro la orden estadistica");
                }
                return Ok(await ROE.EliminarOE(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
    }
}