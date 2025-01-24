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
  

    public class ControladorProveedores : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioProveedores RP;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RP">Interface de RepositorioProveedores</param>

        public ControladorProveedores(IRepositorioProveedores RP)
        {
            this.RP = RP;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos de la tabla</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RP.GetAllProveedores());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Proveedor: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna el Objeto Proveedor</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RP.GetProveedor(id);
                if (resultado.ID_Proveedores == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el proveedor");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Proveedor: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="p">Objeto del tipo Proveedores que se agregara a la base de datos</param>
        /// <returns>Retorna el objeto Proveedores creado</returns>
        [HttpPost]
        public async Task<ActionResult<Proveedores>> Nuevo(Proveedores p)
        {
            try
            {
                if (p == null)
                    return BadRequest();

                string rut = p.Rut_Proveedor;
                string bs = p.ID_Bien_Servicio;
                string res = await RP.Existe(rut,bs);
                if (res == "ok")
                {
                    Proveedores nuevo = await RP.NuevoProveedor(p);
                    return nuevo;
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, res);
                }
             
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para modificar un objeto por ID
        /// </summary>
        /// <param name="P">Objeto Proveedores que se usará para reemplazar su homonimo</param>
        /// <param name="id">Id que representa el objeto Proveedores que se quiere modificar</param>
        /// <returns>Retorna el objeto Proveedores que se modifico</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Proveedores>> Modificar(Proveedores P, int id)
        {
            try
            {
                if (id != P.ID_Proveedores)
                    return BadRequest("La Id no coincide");

              
                return await RP.ModificarProveedor(P);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos "+ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna el Objeto Proveedor</returns>
        [HttpGet("BienServicio/{id:int}")]
        public async Task<ActionResult> GetBienServicio(int id)
        {
            {
                try
                {
                    return Ok(await RP.GetAllProveedoresBienServicio(id));
                }
                catch (Exception ex)
                {
                    // Manejar excepciones generales
                    return StatusCode(StatusCodes.Status500InternalServerError, "No hay proveedores con ese bien servicio: " + ex.Message);
                }
            }

        }
        /// <summary>
        /// Metodo para desactivar un proveedor, para que no se pueda utilizar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Proveedores>> Eliminar(int id)
        {
            try
            {
                var u = RP.GetProveedor(id);
                if (u == null)
                {
                    return NotFound("No se encontro el proveedor");
                }
                return Ok(await RP.EliminarProveedor(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
    }
}