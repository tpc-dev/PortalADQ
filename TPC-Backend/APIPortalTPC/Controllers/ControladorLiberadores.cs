using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
  

    public class ControladorLiberadores : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioLiberadores IRL;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RDU">Interface de IRepositorioDepartamentoUsuario</param>

        public ControladorLiberadores(IRepositorioLiberadores IRL) => this.IRL = IRL;
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna el resultado de GetAll, si se puede seria una lista con todos los Objetos </returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await IRL.GetAll());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener los liberadores: " + ex.Message);
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
                var resultado = await IRL.Get(id);
                if (resultado.Id_Liberador == 0)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el liberador: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="A">Objeto del tipo de Archivo</param>
        /// <returns>Retorna el objeto creado</returns>
        [HttpPost]

        public async Task<ActionResult<Liberadores>> Nuevo(Liberadores L)
        {
            try
            {
                if (L == null)
                    return BadRequest();

                string res = await IRL.Existe(L.Id_Usuario, L.Id_Departamento);
                if (res == "ok")
                {
                    Liberadores nuevo = await IRL.Nuevo(L);
                    return nuevo;
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, res);
                }

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
        public async Task<ActionResult<Liberadores>> Modificar(Liberadores L, int id)
        {
            try
            {
                if (id != L.Id_Liberador)
                    return BadRequest("La Id no coincide");


                string res = await IRL.Existe(L.Id_Usuario, L.Id_Departamento);
                if (res == "ok")
                {
                    return await IRL.Modificar(L);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, res);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos " + ex);
            }
        }
    }
}
