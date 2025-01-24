using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ControladorRecepcion : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioRecepcion RR;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RR">Interface de RepositorioRecepcion</param>

        public ControladorRecepcion(IRepositorioRecepcion RR) => this.RR = RR;
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna el resultado de GetAllArchivos, si se puede seria una lista con todos los Objetos Archivos</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RR.GetAllRecepcion());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener las recepciones: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna el objeto a buscar por Id</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RR.GetRecepcion(id);
                if (resultado.Id_Recepcion == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro la recepcion");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el archivo: " + ex.Message);
            }
        }
        /// <summary>
        /// Crea una nueva recepcion
        /// </summary>
        /// <param name="R"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<ActionResult<Recepcion>> Nuevo(Recepcion R)
        {
            try
            {
                if (R == null)
                    return BadRequest();

                Recepcion nuevo = await RR.NuevaRecepcion(R);
                return nuevo;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }
        /// <summary>
        /// Metodo para modificar una recepcion (el correo con fecha de envio)
        /// </summary>
        /// <param name="R"></param>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Recepcion>> Modificar(Recepcion R, int id)
        {
            try
            {
                if (id != R.Id_Recepcion)
                    return BadRequest("La Id no coincide");

                return await RR.ModificarRecepcion(R);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos " + ex.Message);
            }
        }
    }
}

