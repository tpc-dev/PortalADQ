using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  

    public class ControladorBienServicio : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioBienServicio RBS;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RBS">Interface de RepositorioBienServicio</param>
        public ControladorBienServicio(IRepositorioBienServicio RBS)
        {
            this.RBS = RBS;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna lista con todo los objetos BienServicio</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RBS.GetAllServicio());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el servicio: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscart</param>
        /// <returns>Retorna el objeto cuya id sea la pedida </returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado= await RBS.GetServicio(id);
                if (resultado.ID_Bien_Servicio == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el bien o servicio ");

                return Ok(resultado) ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el servicio: " + ex.Message);
            }
        }
        [HttpGet("Nombre/{id:int}")]
        public async Task<ActionResult> GetNombre(string nombre)
        {
            try
            {
                var resultado = await RBS.GetServicioNombre(nombre);
                if (resultado.ID_Bien_Servicio == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el bien o servicio ");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el servicio: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="bs">Objeto del tipo BienServicio que se añadira a la base de datos</param>
        /// <returns>Retorna el resultado de NuevoBienServicio</returns>
        [HttpPost]
        public async Task<ActionResult<BienServicio>> Nuevo(BienServicio bs)
        {
            try
            {
                if (bs == null)
                    return BadRequest();

                string res = await RBS.Existe(bs.Bien_Servicio);
                if (res != null)
                {
                    if (res.Equals("ok"))
                    {
                        BienServicio nuevoBS = await RBS.NuevoBienServicio(bs);
                        return nuevoBS;
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de "+ex);
            }
        }

        /// <summary>
        /// Metodo asincrónico para modificar un objeto por ID
        /// </summary>
        /// <param name="bs"> Objeto del tipo BienServicio que se usará para modificar su homonimo en la base de datos</param>
        /// <param >name="id">Id del objeto BienServicio a modificar</param>
        /// <returns>Retorna el objeto Modificado</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<BienServicio>> Modificar(BienServicio bs,int id)
        {
            try
            {
                if (id != bs.ID_Bien_Servicio)
                    return BadRequest("La Id no coincide");

                var bienServicioModificar = await RBS.GetServicio(id);

                if (bienServicioModificar == null)
                    return NotFound($"Bien o Servicio con = {id} no encontrado");

                return await RBS.ModificarBien_Servicio(bs);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
        /// <summary>
        /// Metodo que elimina el bien y servicio... eliminar en este caso es quitar la posibilidad de que se use
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<BienServicio>> Eliminar(int id)
        {
            try
            {
                var u = RBS.GetServicio(id);
                if (u == null)
                {
                    return NotFound("No se encontro el bien o servicio");
                }
                return Ok(await RBS.EliminarBien_Servicio(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
   
        
    }
}

 