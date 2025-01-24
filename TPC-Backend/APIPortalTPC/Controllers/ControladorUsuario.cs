using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Net.Http;
using System.Diagnostics;
using System.IO;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 */
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]


    public class ControladorUsuario : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioUsuario RU;
        private readonly InterfaceEnviarCorreo IEC;
        private readonly IRepositorioDepartamentoUsuario IRDU;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RU">Interface de RepositorioUsuario</param>

        public ControladorUsuario(IRepositorioDepartamentoUsuario IRDU, IRepositorioUsuario RU, InterfaceEnviarCorreo IEC)
        {
            this.IEC = IEC;
            this.RU = RU;
            this.IRDU = IRDU;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos del tipo Usuario de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RU.GetAllUsuario());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener los Usuarios: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna el objeto Usuario cuyo Id coincida con el buscado</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RU.GetUsuario(id);
                if (resultado.Id_Usuario == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el usuario");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Usuario: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="U">Objeto del tipo Usuario que se quiere agregar a la base de datos</param>
        /// <returns>Retorna el objeto Usuario agregado</returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> Nuevo(Usuario U)
        {

            try
            {
                if (U == null)
                    return BadRequest();

                string rut = U.Rut_Usuario;
                string res = await RU.Existe(rut, U.Correo_Usuario);
                if (res == "ok")
                {
                    Usuario nuevo = await RU.NuevoUsuario(U);
                    DepartamentoUsuario DU = new DepartamentoUsuario();
                    DU.Id_Departamento = U.Id_Departamento;
                    DU.Id_Usuario = U.Id_Usuario;
                    Console.WriteLine(nuevo.CodigoMFA);
                    await IRDU.Nuevo(DU);
                    nuevo = await RU.ActivarUsuario(nuevo);
                    await RU.ModificarUsuario(nuevo);
                    await IEC.CorreoUsuarioPass(nuevo);
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
        /// Metodo asincrónico para modificar un objeto por ID, contiene un metodo para asegurarse que no exista el objeto
        /// </summary>
        /// <param name="U">Objeto del tipo Usuario que se reemplazará por su homonimo</param>
        /// <param name="id">Id del objeto Usuario a modifcar</param>
        /// <returns>Retorna el nuevo objeto Usuario</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Usuario>> Modificar(Usuario U, int id)
        {
            if (U.Id_Usuario != id)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Id no coincide");
            }
            try
            {
                var Modificar = await RU.GetUsuario(id);

                if (Modificar == null)
                    return NotFound($"Usuario = {id} no encontrado");
                U.CodigoMFA = 1;
                
                return Ok(await RU.ModificarUsuario(U));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }

        }

        /// <summary>
        /// Metodo que descarga un archivo Excel que tiene a todos los Usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet("Imprimir")]
        public async Task<IActionResult> DescargarExcel()
        {
            try
            {
                var LU = await RU.GetAllUsuario();
                // Crear un nuevo archivo Excel
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage())
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ListaUsuario");

                    // Encabezado
                    worksheet.Cells[1, 1].Value = "ID Usuario";
                    worksheet.Cells[1, 2].Value = "Nombre Usuario";
                    worksheet.Cells[1, 3].Value = "Apellido Paterno";
                    worksheet.Cells[1, 4].Value = "Apellido Materno";
                    worksheet.Cells[1, 5].Value = "Rut";
                    worksheet.Cells[1, 6].Value = "Correo Usuario";
                    worksheet.Cells[1, 7].Value = "Contraseña Usuario";
                    worksheet.Cells[1, 8].Value = "Activado";
                    worksheet.Cells[1, 9].Value = "Liberador";
                    worksheet.Cells[1, 10].Value = "En vacaciones";
                    worksheet.Cells[1, 11].Value = "Admin";
                    int row = 2;
                    foreach (var U in LU)
                    {
                        worksheet.Cells[row, 1].Value = U.Id_Usuario;
                        worksheet.Cells[row, 2].Value = U.Nombre_Usuario;
                        worksheet.Cells[row, 3].Value = U.Apellido_paterno;
                        worksheet.Cells[row, 4].Value = U.Apellido_materno;
                        worksheet.Cells[row, 5].Value = U.Rut_Usuario;
                        worksheet.Cells[row, 6].Value = U.Correo_Usuario;
                        worksheet.Cells[row, 7].Value = U.Contraseña_Usuario;
                        worksheet.Cells[row, 8].Value = U.Activado;
                        if (U.Activado == true) worksheet.Cells[row, 8].Value = "Si";
                        else worksheet.Cells[row, 8].Value = "No";
                        worksheet.Cells[row, 9].Value = U.Tipo_Liberador;
                        if (U.Tipo_Liberador == true)
                            worksheet.Cells[row, 9].Value = "Si";
                        else
                            worksheet.Cells[row, 9].Value = "No";
                        worksheet.Cells[row, 10].Value = U.En_Vacaciones;
                        if (U.En_Vacaciones == true) worksheet.Cells[row, 10].Value = "Si";
                        else worksheet.Cells[row, 10].Value = "No";

                        worksheet.Cells[row, 11].Value = U.Admin;
                        if (U.Activado == true) worksheet.Cells[row, 11].Value = "Si";
                        else worksheet.Cells[row, 11].Value = "No";
                        row++;
                    }
                    package.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                }
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "ListaUsuario_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mmss") + ".xlsx";
                return File(memoryStream, contentType, fileName, true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Usuario>> Eliminar(int id)
        {
            try
            {
                Usuario u = await RU.GetUsuario(id);
                if (u == null)
                {
                    return NotFound("No se encontro el Usuario");
                }
                u.Activado = false;
                u.Correo_Usuario = "";
                return Ok(await RU.ModificarUsuario(u));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
    }
}