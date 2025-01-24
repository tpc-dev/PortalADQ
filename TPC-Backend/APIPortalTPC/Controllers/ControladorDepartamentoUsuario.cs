using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{
   
   [Route("api/[controller]")]
    [ApiController]
 

    public class ControladorDepartamentoUsuario : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioDepartamentoUsuario RDU;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RDU">Interface de IRepositorioDepartamentoUsuario</param>

        public ControladorDepartamentoUsuario(IRepositorioDepartamentoUsuario RDU) => this.RDU = RDU;
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna el resultado de GetAll, si se puede seria una lista con todos los Objetos </returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RDU.GetAll());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener las relaciones: " + ex.Message);
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
                var resultado = await RDU.Get(id);
                if (resultado.Id_DepartamentoUsuarios == 0)
                    return NotFound();

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

        public async Task<ActionResult<DepartamentoUsuario>> Nuevo(DepartamentoUsuario A)
        {
            try
            {
                if (A == null)
                    return BadRequest();

                string res = await RDU.Existe(A.Nombre_Usuario, A.Nombre_Departamento);
                if (res == "ok")
                {
                    DepartamentoUsuario nuevo = await RDU.Nuevo(A);
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
        public async Task<ActionResult<DepartamentoUsuario>> Modificar(DepartamentoUsuario A, int id)
        {
            try
            {
                if (id != A.Id_DepartamentoUsuarios)
                    return BadRequest("La Id no coincide");


                string res = await RDU.Existe(A.Nombre_Usuario, A.Nombre_Departamento);
                if (res == "ok")
                {
                    return await RDU.Modificar(A);
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
