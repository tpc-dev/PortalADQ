using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft. AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using Org.BouncyCastle.Crypto;
using System.IO;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
  

    public class ControladorCotizacion : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioCotizacion RC;
        private readonly IRepositorioArchivo IRA;
        private readonly IRepositorioRelacion IRR;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RC">Interface de RepositorioCotizacion</param>

        public ControladorCotizacion(IRepositorioCotizacion RC, IRepositorioArchivo IRA, IRepositorioRelacion IRR)
        {
            this.RC = RC;
            this.IRA = IRA;
            this.IRR = IRR;
        }
        /// <summary>
        /// Metodo que descarga en un Excel todas las cotizaciones
        /// </summary>
        /// <returns></returns>
        [HttpGet("Imprimir")]
        public async Task<IActionResult> DescargarExcel()
        {
            try
            {
                var LRC = await RC.GetAllCotizacion();
                // Crear un nuevo archivo Excel
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage())
                {
                     ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ListaCotizacion");

                    // Encabezado
                    worksheet.Cells[1, 1].Value = "ID Cotizacion";
                    worksheet.Cells[1, 2].Value = "Solicitante";
                    worksheet.Cells[1, 3].Value = "Fecha de Creacion de la cotizacion";
                    worksheet.Cells[1, 4].Value = "Estado";
                    worksheet.Cells[1, 5].Value = "Detalle";
                    worksheet.Cells[1, 6].Value = "Solped";
                    worksheet.Cells[1, 7].Value = "Bien y/o servicio";
                    int row = 2;
                    foreach (var OC in LRC)
                    {
                        worksheet.Cells[row, 1].Value = OC.ID_Cotizacion;
                        worksheet.Cells[row, 2].Value = OC.Id_Solicitante;
                        worksheet.Cells[row, 3].Value = OC.Fecha_Creacion_Cotizacion;
                        worksheet.Cells[row, 3].Style.Numberformat.Format = "yyyy-MM-dd";
                        worksheet.Cells[row, 4].Value = OC.Estado;
                        worksheet.Cells[row, 5].Value = OC.Detalle;
                        worksheet.Cells[row, 6].Value = OC.Solped;
                        worksheet.Cells[row, 7].Value = OC.Bien_Servicio;
                        row++;
                    }



                    package.SaveAs(memoryStream);
                    memoryStream.Position = 0;

                }
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "ListaCotizaciones_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mmss") + ".xlsx";
                return File(memoryStream, contentType, fileName, true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
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

                return Ok(await RC.GetAllCotizacion());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la cotizacion: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna e objeto de la Id a buscar</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado= await RC.GetCotizacion(id);
                if (resultado.ID_Cotizacion == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro la cotizacion");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la cotizacion: " + ex.Message+ id);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="c">Objeto tipo que tiene los datos para crear una cotizacion y ademas el archivo a subir con el nombre de este que se quiere agregar a la base de datos</param>
        /// <returns>Se muestra el objeto agregado</returns>
        [HttpPost]
        public async Task<ActionResult<Cotizacion>> Nuevo(Archivo_Cotizacion c)
 
        {
            try
            {
                if (c == null) return BadRequest();


                Cotizacion nuevac = new Cotizacion
                {
                    Id_Solicitante = c.Id_Solicitante,
                    Fecha_Creacion_Cotizacion = c.Fecha_Creacion_Cotizacion,
                    Estado= c.Estado,
                    Id_Bien_Servicio= c.Id_Bien_Servicio,
                    Solped= c.Solped,
                    Detalle=c.Detalle,
                    Activado = true
                };
                nuevac= await RC.NuevaCotizacion(nuevac);
                using (var memoryStream = new MemoryStream())
                {
                    await c.file.CopyToAsync(memoryStream);
                    //guardamos el archivo y cambiamos el estado de la cotizacion 
                    Archivo A = new Archivo();
                    A.ArchivoDoc = memoryStream.ToArray();
                    string Extension = Path.GetExtension(c.file.FileName);
                    A.NombreDoc = c.fileName.Trim();
                    A = await IRA.NuevoArchivo(A);
                    Relacion R = new Relacion();
                    R.Id_Cotizacion = (nuevac.ID_Cotizacion);
                    R.Id_Archivo = A.Id_Archivo;
                    await IRR.NuevaRelacion(R);
                }
                return nuevac;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        /// <summary>
        /// Metodo asincrónico para modificar un objeto por ID
        /// </summary>
        /// <param name="c">Objeto Cotizacion que tiene el mismo Id que el objeto existente en la base de datos</param>
        /// <param name="id">Id del objeto a cambiar</param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Cotizacion>> Modificar(Cotizacion c, int id)
        {
            try
            {
                var Modificar = await RC.GetCotizacion(id);
                if (Modificar == null)
                    return NotFound($"Cotizacion con = {id} no encontrado");

                return await RC.ModificarCotizacion(c);
            }
            catch (Exception  ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos"+ex);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Cotizacion>> Eliminar(int id)
        {
            try
            {
                var u = RC.GetCotizacion(id);
                if (u == null)
                {
                    return NotFound("No se encontro la cotizacion");
                }
                return Ok(await RC.EliminarCotizacion(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }

        /// <summary>
        /// Obtener el archivo relacionado a la cotizacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Archivo/{id:int}")]
        public async Task<ActionResult<Archivo>> GetArchivo(int id)
        {
            try
            {
                Relacion R = await IRR.GetRelacion(id);
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
