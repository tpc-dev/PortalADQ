using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Mvc;


namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorEnviarCorreo : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly InterfaceEnviarCorreo IEC;
        private readonly IRepositorioProveedores IRP;
        private readonly IRepositorioUsuario IRU;
        private readonly IRepositorioLiberadores IRL;
        private readonly IRepositorioTicket IRT;
        private readonly IRepositorioOrdenCompra IROC;
        private readonly IRepositorioArchivo IRA;
        private readonly IRepositorioRelacion IRR;
        private readonly IRepositorioCotizacion IRC;
        private readonly IRepositorioCorreo IRCo;
        private readonly IRepositorioRecepcion IRRe;
        public ControladorEnviarCorreo(IRepositorioCorreo IRCo, IRepositorioCotizacion IRC,IRepositorioRelacion IRR,IRepositorioArchivo IRA,IRepositorioOrdenCompra IROC, IRepositorioTicket IRT,IRepositorioLiberadores IRL, IRepositorioUsuario IRU, InterfaceEnviarCorreo IEC, IRepositorioProveedores IRP, IRepositorioRecepcion IRRe)
        {
            this.IEC = IEC;
            this.IRP = IRP;
            this.IRU = IRU;
            this.IRL = IRL;
            this.IRA = IRA;
            this.IRT = IRT;
            this.IROC = IROC;
            this.IRR = IRR;
            this.IRC = IRC;
            this.IRCo = IRCo;
            this.IRRe = IRRe;
        }
        /// <summary>
        /// Metodo para enviar a varios proveedores (enviar archivo) las cotizaciones
        /// </summary>
        /// <param name="LID"></param>
        /// <returns></returns>
        [HttpPost("Proveedor")]
        public async Task<IActionResult> EnviarVariosProveedores([FromForm] FormData formData)
        {
            try
            {
                var lis = Array.ConvertAll<string, int>(formData.Proveedor.Split(','), Convert.ToInt32);
                foreach (int ID in lis)
                 {
                    Proveedores P = await IRP.GetProveedor(ID);
                    await IEC.CorreoProveedores(P, formData);
                 }
                //procede a guardar el correo
                if (formData.file == null || formData.file.Length == 0) return Ok("Correos enviados con exito"); // no se guarda archivo
        
                //Se busca la cotizacion
                int idCotizacion = Int32.Parse(formData.Id_Cotizacion);
                Cotizacion Cot = await IRC.GetCotizacion(idCotizacion);
                Cot.Estado = " Enviado";

                await IRC.ModificarCotizacion(Cot);
              
                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }
      
       
        /// <summary>
        /// Metodo que recibe una lista con los ID de los Usuarios, para luego enviarles a sus correos que deben hacer liberaciones (no hay tabla)
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost("VariosLiberadores")]
        public async Task<ActionResult> VariosLiberadores(ListaID LID)
        {
      
       
            int[] lista = LID.Lista;
            string subject = "Recordatorio Urgente: Liberación de Órdenes de Compra Pendientes";
            try
            {
                foreach(int id in lista)
                {
                    //cambiar estado ticket!!!
                    Ticket T = await IRT.GetTicket(id);
                    T.Estado = "Espera liberacion";
                    await IRT.ModificarTicket(T);
            
                    Usuario U = await IRU.GetUsuario(T.Id_U);
       
                    bool enviado = false;
                    List<int> ldep = U.ListaIdDep;
                    int idT = 0;
                    foreach (int dep in ldep)
                    {
                        Liberadores L = await IRL.GetDep(dep);
                        U = await IRU.GetUsuario(L.Id_Usuario);
                        await IEC.CorreoLiberador(U, subject);
                        enviado = true;
                    }
                    if (enviado)
                    {
                        //Dejar que el departamento Todos sea el 9
                        Liberadores lib = await IRL.Get(9);
                        await IEC.CorreoLiberador(U, subject);
                    }
                }
         

                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, " No se encontraron liberadores "+ex);
            }



        }
        


        /// <summary>
        /// Solicitantes para confirmar que tienen OC pendientes de recepcionar, tambien aumenta el contador de correos enviados y crea o actualiza la tabla de recepcion.
        /// </summary>
        /// <param name="LID"></param>
        /// <returns></returns>
        [HttpPost("VariosReceptores")]
        public async Task<ActionResult> VariosReceptores(ListaID LID)
        {
            int[] lista = LID.Lista;
        string subject = "TPC Confirmación de recepción";
            try
            {
                foreach (int i in lista)
                {
                    //se saca la lista con los ID de OC relacionadas al ticket del usuario
                    Ticket T = await IRT.GetTicket(i);
                    Correo C = await IRCo.GetCorreoPorTicket(T.ID_Ticket) ;
     
                    Usuario U = await IRU.GetUsuario(T.Id_U);
                    if (U.Activado)
                    {
                    int id = U.Id_Usuario;
                        await IEC.CorreoRecepciones(U, subject, (int)T.ID_Ticket);
                        //cambiar estado ticket
                        C.CorreosEnviados += 1;
                        C.Activado = true;
                        C.UltimoCorreo = DateTime.Now;
                        await IRCo.ModificarCorreo(C);
                        string res = await IRRe.Existe(C.Id_Correo);

         
                        //si existe recepcion
                        //buscar si esta en recepcion

                        if (res=="ok")
                        {
                            //se crea el nuevo Recepcion
                            Recepcion newR = new Recepcion();
                            newR.Id_Correo = C.Id_Correo;
                            newR.FechaEnvio = C.FechaCreacion;
                            newR.Comentarios = C.detalle;
                            newR.Respuesta = "";
                            await IRRe.NuevaRecepcion(newR);
                        }
                    }
                }
                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, " No se encontraron Receptores "+ex);
            }



        }







    }
}
