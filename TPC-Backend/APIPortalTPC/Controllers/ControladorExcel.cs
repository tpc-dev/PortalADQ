using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;

namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ControladorExcel :ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly InterfaceExcel Excel;
        private readonly IRepositorioCentroCosto IRC;
        private readonly IRepositorioOrdenesEstadisticas IRE;
        private readonly IRepositorioBienServicio IBS;
        private readonly IRepositorioProveedores IRP;
        private readonly IRepositorioBienServicio IRBS;
        private readonly IRepositorioOrdenCompra IROC;
        private readonly IRepositorioTicket IRT;
        public ControladorExcel(IRepositorioTicket IRT,IRepositorioOrdenCompra IROC,IRepositorioBienServicio IRBS,IRepositorioProveedores IRP , InterfaceExcel Excel, IRepositorioCentroCosto IRC, IRepositorioOrdenesEstadisticas IRE, IRepositorioBienServicio IBS)
        {
            this.Excel = Excel;
            this.IRC = IRC;
            this.IRE = IRE;
            this.IBS = IBS;
            this.IRP = IRP;
            this.IRBS = IRBS;
            this.IROC = IROC;
            this.IRT = IRT;
        }

        /// <summary>
        /// Metodo interno para transpasar los Provedores de una base a otra
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("Proveedor")]
        public async Task<ActionResult> ExcelProveedores([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();
                try
                {

                    List<Proveedores> lista = (await Excel.LeerProveedores(Archivo));
                    foreach (Proveedores p in lista)
                    {
                        string res = await IRP.Existe(p.Rut_Proveedor, p.ID_Bien_Servicio);
                        if (res == "ok")
                            await IRP.NuevoProveedor(p);
                    }

                    return Ok(true);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }
            }
        }
        /// <summary>
        /// Metodo que permite leer el excel con el formato que agrega un proveedor a la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpPost("Proveedores")]
        public async Task<ActionResult> ExcelProveedor([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();
                try
                {

                 Proveedores P =(await Excel.LeerExcelProveedor(Archivo));
                    P.ID_Bien_Servicio = "0";
                    //Se crea el bien y servicio
                    await IRP.NuevoProveedor(P);
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    // Manejar excepciones generales
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que lee un archivo excel que tiene orden de compra y lo actualiza en la base de datos
        /// </summary>
        /// <returns></returns>
[HttpPost("OCA")]
public async Task<ActionResult> ActualizarOC([FromForm] IFormFile file)
{
  
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();
                try
                {
     
      
                    return Ok(await Excel.ActualizarOC(Archivo));

                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }
            }
        }
        /// <summary>
        /// Metodo para agregar los Bien y Servicios mediante un excel que tenga dos columnas
        /// </summary>
        /// <returns></returns>
        [HttpPost("BS")]
        public async Task<ActionResult> ExcelBS([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
            { 
                return BadRequest("Please select a file to upload.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();
                try
                {
                 
                    List<BienServicio> lista = (await Excel.LeerBienServicio(Archivo));
                    foreach (BienServicio bs in lista)
                    {
                        string res = await IBS.Existe(bs.Bien_Servicio);
                        if (res == "ok")
                            await IBS.NuevoBienServicio(bs);
                    }

                    return Ok(true);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }

            }
        }
        /// <summary>
        /// Asocia la orden de compra a los tickets
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("poss")]
        public async Task<ActionResult> ExcelPos([FromForm] IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] Archivo = memoryStream.ToArray();
                    return Ok(await Excel.LeerExcelOC(Archivo));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo que lee el archivo de los CentroCosto para agregarlos a la base de datos, tambien añade las ordenes estadisticas!!!
        /// </summary>
        /// <returns></returns>
        [HttpPost("CeCo")]
        public async Task<ActionResult> ExcelCeCo([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }
            try
            {
                using (var memoryStream = new MemoryStream())

                {
                    await file.CopyToAsync(memoryStream);
                    byte[] Archivo = memoryStream.ToArray();

                    {
                        var original = await IRE.GetAllOE();

                        foreach (OrdenesEstadisticas c in original)
                        {
                    
                             await IRE.EliminarOE(c.Id_Orden_Estadistica);
                        }
                        var lc = (await Excel.LeerExcel(Archivo));

                        foreach (OrdenesEstadisticas cc in (List<OrdenesEstadisticas>)lc)
                        {
                            string CecoExiste = await IRC.Existe(cc.Id_Centro_de_Costo);
                            CentroCosto Ceco = new();
                            if (CecoExiste == "ok")
                            {
                                //se crea
                                Ceco.Codigo_Ceco = cc.Id_Centro_de_Costo;
                                Ceco.Nombre = cc.nombreCeCo;
                                Ceco = await IRC.Nuevo_CeCo(Ceco);
                            }
                            else
                            {//se busca
                                Ceco = await IRC.GetCeCo(cc.Id_Centro_de_Costo);

                            }
                            cc.Id_Centro_de_Costo = Ceco.Id_Ceco.ToString();
                            cc.Activado = true;
                            string res = await IRE.Existe(cc.Codigo_OE);
                            if (res == "ok") {
                                cc.Id_CeCo = Ceco.Id_Ceco;
                                await IRE.NuevoOE(cc);
                            }
                            else
                            {
                            OrdenesEstadisticas old = await IRE.GetOECode(cc.Codigo_OE);
                            cc.Activado = true;
                            cc.Id_CeCo = old.Id_CeCo;
                            cc.Id_Orden_Estadistica = old.Id_Orden_Estadistica;
                            await IRE.ModificarOE(cc);
                            }
                        }
                    }
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
            }
        }
    }
}

     
