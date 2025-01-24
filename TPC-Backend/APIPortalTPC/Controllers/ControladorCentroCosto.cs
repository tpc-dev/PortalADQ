using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Mvc;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
  

    public class ControladorCentroCosto : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioCentroCosto RC;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RC">Interface de RepositorioCentroCosto</param>

        public ControladorCentroCosto(IRepositorioCentroCosto RC)
        {
            this.RC = RC;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos Centro_de_costo de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RC.GetAllCeCo());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Centro de Costo: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna el objeto a buscar</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RC.GetCeCo(id);
                if (resultado.Id_Ceco == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el centro de costo");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Centro de Costo: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="A">Objeto del tipo Centro_de_costo que va a ser agregado a la base de datos</param>
        /// <returns>Retorna el objeto que va a ser agregado</returns>
        [HttpPost]
        public async Task<ActionResult<CentroCosto>> Nuevo(CentroCosto CeCo)
        {
            try
            {
                if (CeCo == null)
                    return BadRequest();

                string res = await RC.Existe(CeCo.Codigo_Ceco);
                if (res != null)
                {
                    if (res.Equals("ok"))
                    {
                        CentroCosto nuevoCeCo = await RC.Nuevo_CeCo(CeCo);
                        return nuevoCeCo;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, res);
                    }

                }
                else
                {
                    return StatusCode(StatusCodes.Status418ImATeapot, "Valor nulo!?");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        /// <summary>
        /// Metodo asincrónico para modificar un objeto por ID
        /// </summary>
        /// <param name="C">Objeto de Centro_de_costo que va a reemplazar su homonimo de la base de datos</param>
        /// <param name="id">Id del objeto a buscar para cambiar</param>
        /// <returns>el objeto que va a ser reemplazado</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CentroCosto>> Modificar(CentroCosto C, int id)
        {
            try
            {
                if (id != C.Id_Ceco)
                    return BadRequest("La Id no coincide");

                var Modificar = await RC.GetCeCo(id);

                if (Modificar == null)
                    return NotFound($"Centro de Costo con = {id} no encontrado");

                return await RC.ModificarCeCo(C);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
        /// <summary>
        /// metodo que cambia el estado del Centro de costo activado a falso para que no sea usable
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CentroCosto>> Eliminar(int id)
        {
            try
            {
                var u = RC.GetCeCo(id);
                if (u == null)
                {
                    return NotFound("No se encontro el Usuario");
                }
                return Ok(await RC.EliminarCeCo(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }

    }
}