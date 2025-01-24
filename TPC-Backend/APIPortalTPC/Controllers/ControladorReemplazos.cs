using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Processing;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
  

    public class ControladorReemplazos : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioReemplazos RR;
        private readonly IRepositorioUsuario RU;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RR">Interface de RepositorioReemplazos</param>

        public ControladorReemplazos(IRepositorioReemplazos RR, IRepositorioUsuario RU)
        {
            this.RR = RR;
            this.RU = RU;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos del tipo Reemplazos</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RR.GetAllRemplazos());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Reemplazo: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id"> Id del objeto a buscar</param>
        /// <returns>Retorna el objeto Reemplazos cuya Id coincida con la que se busca</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RR.GetReemplazo(id);
                if (resultado.ID_Reemplazos == 0)
                    return StatusCode(StatusCodes.Status404NotFound,"No se encontro el reemplazo");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Reemplazo: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="R">Objeto del tipo Reemplazos que se quiere agregar a la base de datos</param>
        /// <returns>Retorna el objeto creado</returns>
        [HttpPost]
        public async Task<ActionResult<Reemplazos>> Nuevo(Reemplazos R)
        {
            try
            {
                if (R == null)
                    return BadRequest();

                Reemplazos nuevo = await RR.NuevoReemplazos(R);
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
        /// <param name="R">Objeto del tipo Reemplazos que se usará para cambiar a su homonimo</param>
        /// <param name="id">Id del objeto a modificar</param>
        /// <returns>Retorna el objeto Reemplazos modificado</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Reemplazos>> Modificar(Reemplazos R, int id)
        {
            try
            {
                if (id != R.ID_Reemplazos)
                    return BadRequest("La Id no coincide");

                var Modificar = await RR.GetReemplazo(id);

                if (Modificar == null)
                    return NotFound($"Reemplazo con = {id} no encontrado");

                Modificar.N_IdR = R.N_IdR;
                Modificar.Comentario = R.Comentario;
       

                return await RR.ModificarReemplazos(Modificar);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos "+ex.Message);
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Reemplazos>> FinRemplazo(int id)
        {
            try
            {
                //sacar a ambos usuarios

                Reemplazos R = await RR.GetReemplazo(id);
                //luego desactivar Reemplazo
                R.Valido = false;

                Usuario U = await RU.GetUsuario(R.N_IdV);

                if (U.Id_Usuario != 0)
                {
                    await RU.ModificarUsuario(U);
                    R = await RR.ModificarReemplazos(R);
          
                    return Ok(R);
                }
                return BadRequest("No se modifico");
            }   
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos " + ex.Message);
            }


        }
    }
}