using APIPortalTPC.Repositorio;
using Microsoft.AspNetCore.Mvc;
using BaseDatosTPC;
using ClasesBaseDatosTPC;

namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorCorreo : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioCorreo RC;
        public ControladorCorreo(IRepositorioCorreo RC) {
            this.RC = RC;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RC.GetAllCorreo());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener los correo : " + ex.Message);
            }
        }
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RC.GetCorreo(id);
                if (resultado.Id_Correo == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el correo");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el correo: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Correo>> Nuevo(Correo C)
        {

            try
            {
                if (C == null)
                    return BadRequest();

                Correo  nuevo = await RC.NuevoCorreo(C);
                return Ok(nuevo);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }
        /// <summary>
        /// Modifiiicel correo por ID
        /// </summary>
        /// <param name="C"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Correo>> Modificar(Correo C, int id)
        {
            if (C.Id_Correo != id)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Id no coincide");
            }
            try
            {
                var Modificar = await RC.GetCorreo(id);

                if (Modificar == null)
                    return NotFound($"Usuario = {id} no encontrado");

                return Ok(await RC.ModificarCorreo(C));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
    }

  
}
