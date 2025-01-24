using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]


    public class ControladorDepartamento : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita
        //inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioDepartamento RD;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RD">Interface de RepositorioDepartamento</param>

        public ControladorDepartamento(IRepositorioDepartamento RD)
        {
            this.RD = RD;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RD.GetAllDepartamento());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Departamento: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id a buscar del objeto</param>
        /// <returns>Retorna un objeto del tipo Departamento cuyo Id sea el buscado</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RD.GetDepartamento(id);
                if (resultado.Id_Departamento == 0)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Departamento: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="D">Objeto Departamento que se agregara a la base de datos</param>
        /// <returns>Retorna el objeto a añadir</returns>
        [HttpPost]
        public async Task<ActionResult<Departamento>> Nuevo(Departamento D)
        {

                try
                {
                    if (D == null)
                        return BadRequest();

                    string res = await RD.Existe(D.Nombre);
                    if (res == "ok")
                    {
                        Departamento nuevo = await RD.NuevoDepartamento(D);
                        return nuevo;
                    }
                    else { 
                        return StatusCode(StatusCodes.Status400BadRequest, res); 
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
        /// <param name="D">Objeto Departamento que va a reemplazar a su homonido en la base de datos</param>
        /// <param name="id">Id del objeto a reemplazar</param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Departamento>> Modificar(Departamento D, int id)
        {
            try
            {
                if (id != D.Id_Departamento)
                    return BadRequest("La Id no coincide");

                var Modificar = await RD.GetDepartamento(id);

                if (Modificar == null)
                    return NotFound($"Departamento con = {id} no encontrado");

                return await RD.ModificarDepartamento(D);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos"+ex);
            }

        }
        /// <summary>
        /// Metodo que "elimina" un departamento, aunque lo que hace es que no sea accesible
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Departamento>> Eliminar(int id)
        {
            try
            {
                var u = RD.GetDepartamento(id);
                if (u == null)
                {
                    return NotFound("No se encontro el departamento");
                }
                return Ok(await RD.EliminarDepartamento(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
    }
}