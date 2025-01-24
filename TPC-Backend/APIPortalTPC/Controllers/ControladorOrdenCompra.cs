using APIPortalTPC.Repositorio;
using BaseDatosTPC;

using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Linq.Expressions;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
  

    public class ControladorOrdenCompra : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioOrdenCompra ROC;
        private readonly IRepositorioTicket RT;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="ROC">Interface de RepositorioOrdenCompra</param>

        public ControladorOrdenCompra(IRepositorioTicket RT,IRepositorioOrdenCompra ROC)
        {
            this.ROC = ROC;
            this.RT = RT;
        }
        /// <summary>
        /// Metodo que va a imprimir un excel usando la lista de orden compra ya filtrada
        /// </summary>
        /// <returns></returns>
        [HttpGet("Imprimir")]

        public async Task<IActionResult> DescargarExcel()
        {
          try{  
                var LOC = await ROC.GetAllOC();
                // Crear un nuevo archivo Excel
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ListaOrdenCompras");
                    // Encabezado
                    worksheet.Cells[1, 1].Value = "Ticket";
                    worksheet.Cells[1, 2].Value = "Numero de Orden Compra";
                    worksheet.Cells[1, 3].Value = "Fecha de Recepcion";
                    worksheet.Cells[1, 4].Value = "Texto";
                    worksheet.Cells[1, 5].Value = "Ciclicidad";
                    worksheet.Cells[1, 6].Value = "Posicion";
                    worksheet.Cells[1, 7].Value = "Cantidad";
                    worksheet.Cells[1, 8].Value = "Moneda";
                    worksheet.Cells[1, 9].Value = "Precio Neto";
                    worksheet.Cells[1, 10].Value = "Proveedor";
                    worksheet.Cells[1, 11].Value = "Material";
                    worksheet.Cells[1, 12].Value = "Valor Neto";
                    worksheet.Cells[1, 13].Value = "Recepcionada";
                    // Filas
                    int row = 2;
                    foreach (var OC in LOC)
                    {
                        worksheet.Cells[row, 1].Value = OC.Id_Ticket;
                        worksheet.Cells[row, 2].Value = OC.Numero_OC;
                        worksheet.Cells[row, 3].Value = OC.Fecha_Recepcion;
                        worksheet.Cells[row, 3].Style.Numberformat.Format = "yyyy-MM-dd";
                        worksheet.Cells[row, 4].Value = OC.Texto;
                        if (OC.IsCiclica == true)
                            worksheet.Cells[row, 5].Value = "Si";
                        else
                            worksheet.Cells[row, 5].Value = "No";
                        worksheet.Cells[row, 6].Value = OC.posicion;
                        worksheet.Cells[row, 7].Value = OC.Cantidad;
                        worksheet.Cells[row, 8].Value = OC.Mon;
                        worksheet.Cells[row, 9].Value = OC.PrcNeto;
                        worksheet.Cells[row, 10].Value = OC.Proveedor;
                        worksheet.Cells[row, 11].Value = OC.Material;
                        worksheet.Cells[row, 12].Value = OC.ValorNeto;
                        if (OC.Recepcion == true)
                            worksheet.Cells[row, 13].Value = "Si";
                        else
                            worksheet.Cells[row, 13].Value = "No";
                        row++;
                    }
                        package.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                }
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "ListaOrdenCompra_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mmss") + ".xlsx";
                return File(memoryStream, contentType, fileName,true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error de "+ ex);
            }


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
                return Ok(await ROC.GetAllOC());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la Orden de compra: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await ROC.GetOC(id);
                if (resultado.Id_Orden_Compra == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro la orden de compra");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la Orden de compra: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="OC">Objeto OrdenCompra que se agregará a la base</param>
        /// <returns>El obejeto creado</returns>
        [HttpPost]
        public async Task<ActionResult<OrdenCompra>> Nuevo(OrdenCompra OC)
        {
            try
            {
                if (OC == null)
                    return BadRequest();

                OrdenCompra nuevo = await ROC.NuevoOC(OC);
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
        /// <param name="OC">Objeto OrdenCompra que se usará para reemplazar a su homonimo </param>
        /// <param name="id">Id del objeto a reemplazar</param>
        /// <returns>Retorna el objeto modificado</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrdenCompra>> Modificar(OrdenCompra OC, int id)
        {
            try
            {

                if (id != OC.Id_Orden_Compra)
                    return BadRequest("La Id no coincide");

  
                OC =await ROC.ModificarOC(OC);
  

                var Modificar = await ROC.GetOC(id);

                if (Modificar == null)
                    return NotFound($"Orden de compra con = {id} no encontrado");

                int id_T = (int)Modificar.Id_Ticket;
                //aprovecha de modificar los tickets!!!
   
                await RT.ActualizarEstadoTicket(id_T);
                return Ok("Cambios realizados");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos "+ex.Message);
            }
        }
        /// <summary>
        /// Metodo para eliminar una Orden de Compra y asi no se pueda acceder
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<OrdenCompra>> Eliminar(int id)
        {
            try
            {
                var u = ROC.GetOC(id);
                if (u == null)
                {
                    return NotFound("No se encontro la Orden de compra");
                }
                return Ok(await ROC.EliminarOC(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }

    }
}