using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
  

    public class ControladorTicket : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioTicket RT;
        private readonly IRepositorioOrdenCompra ROC;
        private readonly IRepositorioCorreo IRC;
        private readonly IRepositorioIdRelacionTicket IRRT;
        private readonly IRepositorioArchivo IRA;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RT">Interface de RepositorioTicket</param>

        public ControladorTicket(IRepositorioArchivo IRA, IRepositorioCorreo IRC,IRepositorioTicket RT, IRepositorioOrdenCompra ROC, IRepositorioIdRelacionTicket IRRT)
        {
            this.RT = RT;
            this.ROC = ROC;
            this.IRC = IRC;
            this.IRRT = IRRT;
            this.IRA = IRA;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos de la clase Ticket de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RT.GetAllTicket());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Ticket: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna el objeto Ticket creado</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RT.GetTicket(id);
                if (resultado.ID_Ticket == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el ticket");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Ticket: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="T">Objeto del tipo Ticket que se quiere agregar a la base de datos</param>
        /// <returns>Retorna el objeto Ticket que se agrego a la base de datos</returns>
        [HttpPost]
        public async Task<ActionResult<Ticket>> Nuevo(Archivo_Ticket T)
        {
            Console.WriteLine("Estoy aqui");
            try
            {
                if (T == null)
                    return BadRequest();

                Ticket nuevo =new Ticket
                {
                    Estado= T.Estado,
                    Fecha_Creacion_OC=T.Fecha_Creacion_OC,
                    Id_Usuario=T.Id_Usuario,
                    ID_Proveedor= T.ID_Proveedor, 
                    Fecha_OC_Recepcionada= T.Fecha_OC_Recepcionada, 
                    Fecha_OC_Enviada= T.Fecha_OC_Enviada, 
                    Fecha_OC_Liberada= T.Fecha_OC_Liberada, 
                    Detalle= T.Detalle ?? " ", 
                    Solped=T.Solped ?? 0, 
                    Id_OE= T.Id_OE
                };
                nuevo = await RT.NewTicket(nuevo);

                using (var memoryStream = new MemoryStream())
                {
                    await T.file.CopyToAsync(memoryStream);
                    //guardamos el archivo y cambiamos el estado de la cotizacion 
                    Archivo A = new Archivo();
                    A.ArchivoDoc = memoryStream.ToArray();
                    A.NombreDoc = T.fileName.Trim();
                    A = await IRA.NuevoArchivo(A);
                    Id_RelacionTicket R = new Id_RelacionTicket();
                    //pasaaaaaaarl ID Ticket y archivo
                    R.Id_Ticket = (nuevo.ID_Ticket);
                    R.Id_Archivo = A.Id_Archivo;
                    await IRRT.NuevaRelacion(R);
                }
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
        /// <param name="T">Objeto del tipo Ticket que se quiere reemplazar por su homonimo</param>
        /// <param name="id">Id del objeto Ticket a modificar</param>
        /// <returns>Objeto Ticket modificado</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Ticket>> Modificar(Ticket T, int id)
        {
            try
            {
                if (id != T.ID_Ticket)
                    return BadRequest("La Id no coincide");

                var Modificar = await RT.GetTicket(id);

                if (Modificar == null)
                    return NotFound($"Centro de Costo con = {id} no encontrado");

                Correo C = new Correo();
                await RT.ModificarTicket(T);
                if(T.Estado == "OC Enviada")
                {

                    //crear el objeto Correo
               
                    C.Id_Ticket = T.ID_Ticket;
                    C.Numero_OC = T.Numero_OC;
                    C.Proveedor = T.ID_Proveedor;
                    C.CeCo = T.Id_OE;
                    C.CorreosEnviados = 0;
                    C.detalle = T.Detalle;

                    string res = await IRC.Existe(T.ID_Ticket);
                    if (res.Equals("ok"))
                    {
                        await IRC.NuevoCorreo(C);
                    }
                }else
                    if (T.Estado == "OC Recepcionada"|| T.Estado == "OC Cancelada")
                {
                  C = await IRC.GetCorreoPorTicket((T.ID_Ticket));
                    if (C.Id_Ticket != 0)
                    {
                        C.Activado = false;

                        await IRC.ModificarCorreo(C);
                    }

                }
                return T;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos "+ex.Message);
            }
        }
        /// <summary>
        /// Metodo para actualizar el estado del ticket, esto se hace revisando las ordenes de compra asociadas, luego se debe 
        /// actualizar las Ordenes de compra si Estado ticket es OC Recepcionada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Estado/{id:int}")]    
        public async Task<ActionResult<Ticket>> EstadoTicket(int id)
        {
            try
            {
                var Ticket = await RT.ActualizarEstadoTicket(id);
                if (Ticket.ID_Ticket != 0) 
                {
                    if ( Ticket.Estado.Equals("OC Parcial") || Ticket.Estado.Equals("Espera de liberacion", StringComparison.OrdinalIgnoreCase))
                    {
                        var OC = await ROC.GetAllOCTicket(Ticket.ID_Ticket);
                        foreach (OrdenCompra cambia in OC)
                        {
                            await ROC.ModificarOC(cambia);
                        }
                        return Ticket;
                    }
                }
                    return NotFound("Ticket no encontrado");
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status400BadRequest,"Error no se de que "+ex);
            }
            
        }
        /// <summary>
        /// Deja el estado "invisible" para usar el ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Ticket>> Eliminar(int id)
        {
            try
            {
                var u = RT.GetTicket(id);
                if (u == null)
                {

                    return NotFound("No se encontro el ticket");
                }
                Correo C = await IRC.GetCorreoPorTicket((id));
                if (C.Id_Ticket != 0)
                {
                    C.Activado = false;

                    await IRC.ModificarCorreo(C);
                }
                return Ok(await RT.EliminarTicket(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
        /// <summary>
        /// Permite ver las ordenes de compra asociadas al ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ListaOC/{id:int}")]
        public async Task<ActionResult<OrdenCompra>> ListaOC(int id)
        {
            try
            {
                var lista = await ROC.GetAllOC();
                var ticket = await RT.GetTicket(id);
                List<OrdenCompra> loc = new List<OrdenCompra>();
                if (ticket.ID_Ticket == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                foreach (OrdenCompra OC in lista)
                    if (OC.Id_Ticket == ticket.ID_Ticket)
                        loc.Add(OC);
                if(loc == null)
                    return BadRequest();

                return Ok(loc);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        /// <summary>
        /// Muestra los ticket del usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Usuario/{id:int}")]
        public async Task<ActionResult> GetAllUsuario(int id)
        {
            try
            {
                return Ok(await RT.GetAllTicketUsuario(id));
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Ticket: " + ex.Message);
            }
        }

        /// <summary>
        /// Dado un Id_Ticket, modifica todos las ordens de compras asociadas y cambia el estado a OC Recepcionada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost("RecepcionTotal/{id:int}")]
        public async Task<ActionResult> RecepcionTotal(int id)
        {
       
            try
            {
                var ListaOC = await ROC.GetAllOCTicket(id);
                Ticket T = await RT.GetTicket(id);
         
                T.Estado = "OC Recepcionada";
                foreach(OrdenCompra OC in ListaOC)
                {
                    //Pasar estado a true
                    OC.Recepcion = true;
                    await ROC.ModificarOC(OC);
                }
                await RT.ModificarTicket(T);

                //comprobar que exista el correo correspondiente
                Correo C = await IRC.GetCorreoPorTicket((T.ID_Ticket));
                if (C.Id_Ticket!=0)
                {
                    C.Activado = false;

                    await IRC.ModificarCorreo(C);
                }
                return Ok("Recepcionado con exito");
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error : " + ex.Message);
            }
        }
        [HttpGet("Archivo/{id:int}")]
        public async Task<ActionResult<Archivo>> GetArchivo(int id)
        {
            try
            {
                Id_RelacionTicket R = await IRRT.GetRelacion(id);
                Archivo A = await IRA.GetArchivo((int)R.Id_Archivo);

                return A;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sacando archivo: " + ex);
            }
        }


    }
}