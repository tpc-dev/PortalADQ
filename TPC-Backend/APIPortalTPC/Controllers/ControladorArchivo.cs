using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Mvc;

/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class ControladorArchivo : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioArchivo RA;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RA">Interface de RepositorioArchivo</param>

        public ControladorArchivo(IRepositorioArchivo RA) => this.RA = RA;
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna el resultado de GetAllArchivos, si se puede seria una lista con todos los Objetos Archivos</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RA.GetAllArchivo());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el archivo: " + ex.Message);
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
                var resultado = await RA.GetArchivo(id);
                if (resultado.Id_Archivo == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el archivo");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el archivo: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="A">Objeto del tipo de Archivo</param>
        /// <returns>Retorna el objeto creado</returns>
        [HttpPost]
        public async Task<ActionResult<Archivo>> Nuevo(Archivo A)
        {
            try
            {
                if (A == null)
                    return BadRequest();

                Archivo nuevo = await RA.NuevoArchivo(A);
                return nuevo;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        /// <summary>
        /// Metodo asincrónico para modificar un objeto por ID, si el objeto coincide con la Id se procede a agregarse a la base de datos
        /// </summary>
        /// <param name="A">Objeto del tipo Archivo que se quiere modificar</param>
        /// <param name="id">Id del objeto que se quiere buscar</param>
        /// <returns>Regresa el Objeto modificado</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Archivo>> Modificar(Archivo A, int id)
        {
            try
            {
                if (id != A.Id_Archivo)
                    return BadRequest("La Id no coincide");

                var Modificar = await RA.GetArchivo(id);

                if (Modificar == null)
                    return NotFound($"Archivo con = {id} no encontrado");

                return await RA.ModificarArchivo(A);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos "+ ex.Message);
            }
        }

    }
}