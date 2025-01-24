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

    public class ControladorRelacion : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioRelacion RR;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RR">Interface de RepositorioRelacion</param>

        public ControladorRelacion(IRepositorioRelacion RR)
        {
            this.RR = RR;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna lista con todos los objetos Relacion</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RR.GetAllRelacion());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la Relación: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna el objeto Relacion que coincida con la Id requerida</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RR.GetRelacion(id);
                if (resultado.Id_Relacion == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro la relacion");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la Relación: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="R">Objeto del tipo Relacion que se agregará a la base de datos</param>
        /// <returns>Retorna el objeto creado</returns>
        [HttpPost]
        public async Task<ActionResult<Relacion>> Nuevo(Relacion R)
        {
            try
            {
                if (R == null)
                    return BadRequest();

                Relacion nuevo = await RR.NuevaRelacion(R);
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
        /// <param name="R">Objeto del tipo Relacion que se usara para cambiar a su homonimo</param>
        /// <param name="id">Id del objeto a modificar</param>
        /// <returns>Retorna el objeto Relacion modificado</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Relacion>> Modificar(Relacion R, int id)
        {
            try
            {
                if (id != R.Id_Relacion)
                    return BadRequest("La Id no coincide");

                var Modificar = await RR.GetRelacion(id);

                if (Modificar == null)
                    return NotFound($"Centro de Costo con = {id} no encontrado");

                return await RR.ModificarRelacion(R);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
    }
}