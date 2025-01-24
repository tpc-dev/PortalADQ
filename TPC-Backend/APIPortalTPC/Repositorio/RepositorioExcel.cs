using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using System.Globalization;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.SS.Formula.Functions;



namespace APIPortalTPC.Repositorio
{
    public class RepositorioExcel : InterfaceExcel
    {
        private string Conexion;
        private readonly IRepositorioTicket IRT;
        private readonly IRepositorioOrdenCompra IROC;
        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioExcel(AccesoDatos CD, IRepositorioTicket iRT, IRepositorioOrdenCompra iROC)
        {
            Conexion = CD.ConexionDatosSQL;
            IRT = iRT;
            IROC = iROC;
        }
        /// <summary>
        /// Metodo que realiza la conexión a la base de datos
        /// </summary>
        /// <returns>La conexión</returns>
        private SqlConnection conectar()
        {
            return new SqlConnection(Conexion);
        }
        /// <summary>
        /// Metodo que lee el archivo excel que posee un proveedor, retorna un objeto Proveedores
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Proveedores> LeerExcelProveedor(byte[] archivo)
        {
            // Establecer el contexto de la licencia
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Proveedores P = new Proveedores();
            using (var memoryStream = new MemoryStream(archivo))
            using (ExcelPackage package = new ExcelPackage(memoryStream))
            {

                {
                    // Seleccionar la primera hoja del archivo
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    // Leer una celda específica (por ejemplo, B2)
                    P.Rut_Proveedor = worksheet.Cells["C19"].Text; //RUT PROVEEDOR CELDA C19
                    P.Razon_Social = worksheet.Cells["C17"].Text; // RAZON SOCIAL CELDA C17 
                    P.Nombre_Fantasia = worksheet.Cells["C17"].Text;  // NOMBRE FANTASIA CELDA C17
                    P.Direccion = worksheet.Cells["C23"].Text;   // DIRRECION  CELDA C23
                    P.Comuna = worksheet.Cells["C25"].Text;  // COMUNA CELDA C25
                    P.Correo_Proveedor = worksheet.Cells["C27"].Text;  // CORREO PROVEEDOR CELDA C27
                    if (worksheet.Cells["C21"].Value != null) P.Telefono_Proveedor = worksheet.Cells["C21"].Value.ToString();  // TELEFONO PROVEEDOR CELDA C21
                    P.Cargo_Representante = worksheet.Cells["C35"].Text;  // CARGO REPRESENTANTE CELDA C35
                    P.Nombre_Representante = worksheet.Cells["C31"].Text;  // NOMBRE REPRESENTANTE CELDA C31
                    P.Email_Representante = worksheet.Cells["F33"].Text;  // EMAIL DE REPRESENTANTE CELDA F33
                    if (worksheet.Cells["C57"].Value != null) P.N_Cuenta = worksheet.Cells["C57"].Value.ToString();  // NUMERO DE CUENTA CELDA C57
                    P.Banco = worksheet.Cells["C51"].Text;  // BANCO CELDA C51
                    P.Swift = worksheet.Cells["C55"].Text;  // SWIFT 1 CELDA C55
                    P.ID_Bien_Servicio = "0";
                }

                return P;

            }

        }

        /// <summary>
        /// Metodo que lee el excel con los centros de costo para ser añadidos a una lista de objeto CentroCosto
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<List<CentroCosto>> LeerExcelCeCo(byte[] archivo)
        {
            var centrosCostos = new List<CentroCosto>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var memoryStream = new MemoryStream(archivo))
            using (ExcelPackage package = new ExcelPackage(memoryStream))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Hoja1") ??
                         package.Workbook.Worksheets.FirstOrDefault();
                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {
                    string Namea = "";
                    if (worksheet.Cells[row, 2].Value is string)
                    {
                        Namea = worksheet.Cells[row, 2].Value.ToString();

                    }
                    var centroCosto = new CentroCosto
                    {
                        Codigo_Ceco = worksheet.Cells[row, 1].Value.ToString(),
                        Nombre = Namea
                    };
                    centrosCostos.Add(centroCosto);
                }
            }
            return centrosCostos;
        }

        /// <summary>
        /// Metodo que lee el excel con los centros de costo para ser añadidos a una lista de objeto CentroCosto
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<List<OrdenesEstadisticas>> LeerExcel(byte[] archivo)
        {
            List<OrdenesEstadisticas> LOE = new List<OrdenesEstadisticas>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            using (var memoryStream = new MemoryStream(archivo))
            using (ExcelPackage package = new ExcelPackage(memoryStream))

            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Hoja1") ??
                         package.Workbook.Worksheets.FirstOrDefault();
                int rowCount = worksheet.Dimension.Rows+3;
                for (int row = 4; row <= rowCount; row++)
                    {
                    if (row != 4)
                    {
                        OrdenesEstadisticas OE = new();
                        OE.Codigo_OE = worksheet.Cells[row, 2].Value?.ToString();
                        OE.Nombre = worksheet.Cells[row, 3].Value?.ToString();
                        OE.Id_Centro_de_Costo = worksheet.Cells[row, 4].Value?.ToString();
                        OE.nombreCeCo = worksheet.Cells[row, 5].Value?.ToString();
                        LOE.Add(OE);
                     
                    }
                    }
                }

            return LOE;
        }
        /// <summary>
        /// Metodo que lee el excel de ordenes de compras y actualiza la base de datos para cambiar los procesos
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> ActualizarOC(byte[] archivo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Variables para almacenar datos de las columnas C y Q
            string columnaC;
            string columnaQ;
            DateTime hoy = (DateTime.Today);
            // Cargar el archivo Excel
            using (var memoryStream = new MemoryStream(archivo))
            using (ExcelPackage package = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Primera hoja del archivo
                    int rowCount = worksheet.Dimension.Rows;
                for (int row = 6; row <= rowCount; row++)
                {
                 
                    if (worksheet.Cells[row, 3].Value != null || worksheet.Cells[row, 17].Value != null)
                    {
                        // Agregar el valor de la columna C (Documentos) o una cadena vacía si es nulo, este es el numero de OC
                        columnaC = worksheet.Cells[row, 3].Value.ToString() ?? string.Empty;


                    // Agregar el valor de la columna Q (Denominación StatLib) o una cadena vacía si es nulo, este es el Estado
                    columnaQ = worksheet.Cells[row, 17].Text ?? string.Empty;

                    SqlConnection sqlConexion = conectar();
                    SqlCommand? Comm = null;
                    SqlDataReader reader = null;
                    try
                    {
                        sqlConexion.Open();
                        Comm = sqlConexion.CreateCommand();
                        Comm.CommandText = "UPDATE dbo.Ticket SET  " +
                            "Fecha_OC_Liberada = @FL , " +
                            "Estado = @E " +
                            "WHERE N_OC = @ID_Ticket ";
                        Comm.CommandType = CommandType.Text;
                            if (columnaQ == "Liberación concluida")
                            {
                                Comm.Parameters.Add("@FL", SqlDbType.DateTime).Value = hoy;
                                columnaQ = "OC Liberada";
                            }
                            else
                            {
                                Comm.Parameters.Add("@FL", SqlDbType.DateTime).Value = DBNull.Value;
                                columnaQ = "Espera liberacion";
                            }
                        Comm.Parameters.Add("@E", SqlDbType.VarChar, 500).Value = columnaQ;
                        Comm.Parameters.Add("@ID_Ticket", SqlDbType.BigInt).Value = int.Parse(columnaC);

                        reader = await Comm.ExecuteReaderAsync();

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
                    }
                    finally
                    {
                        reader?.Close();
                        Comm?.Dispose();
                        sqlConexion.Close();
                        sqlConexion.Dispose();
                    }

                }
                }
            }

            return "listo";
        }
        /// <summary>
        /// Metodo para leer un excel y añadirlo a la base de datos en la tabla BienServicio... inaccesible
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public async Task<List<BienServicio>> LeerBienServicio(byte[] archivo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var lista = new List<BienServicio>();
            string hoja = "Hoja1";
            int columna = 2;
            using (var memoryStream = new MemoryStream(archivo))
            using (ExcelPackage package = new ExcelPackage(memoryStream))

            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[hoja];


                int rowCount = worksheet.Dimension.Rows;
                List<string> datosColumna = new List<string>();
                for (int row = 2; row <= rowCount; row++)
                {
                    string valorCelda = worksheet.Cells[row, columna].Value?.ToString();
                    BienServicio bienServicio = new();
                    bienServicio.Bien_Servicio = valorCelda;
                    lista.Add(bienServicio);
                }
            }
            return lista;
        }
        /// <summary>
        /// Metodo para agregar varios Proveedores usando un excel... es inaccesible y se uso para transladar los datos de 
        /// la base de datos anterior
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public async Task<List<Proveedores>> LeerProveedores(byte[] archivo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<Proveedores> lista = new List<Proveedores>();
            string hoja = "Hoja1";

            using (var memoryStream = new MemoryStream(archivo))
            using (ExcelPackage package = new ExcelPackage(memoryStream))


            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[hoja];


                int rowCount = worksheet.Dimension.Rows;
                List<string> datosColumna = new List<string>();
                for (int row = 2; row <= rowCount; row++)
                {

                    Proveedores P = new Proveedores();
                    P.ID_Bien_Servicio = worksheet.Cells[row, 1].Value?.ToString();
                    P.Rut_Proveedor = worksheet.Cells[row, 2].Value?.ToString();
                    P.Razon_Social = worksheet.Cells[row, 3].Value?.ToString();
                    P.Nombre_Fantasia = worksheet.Cells[row, 4].Value?.ToString();
                    P.Direccion = worksheet.Cells[row, 5].Value?.ToString();
                    P.Comuna = worksheet.Cells[row, 6].Value?.ToString();
                    P.Telefono_Proveedor = worksheet.Cells[row, 7].Value?.ToString();
                    P.Correo_Proveedor = worksheet.Cells[row, 8].Value?.ToString();
                    P.Email_Representante = worksheet.Cells[row, 9].Value?.ToString();
                    P.Nombre_Representante = worksheet.Cells[row, 10].Value?.ToString();
                    P.Estado = Convert.ToBoolean(worksheet.Cells[row, 11].Value);
                    P.N_Cuenta = worksheet.Cells[row, 13].Value?.ToString();
                    if (P.Rut_Proveedor != null)
                        lista.Add(P);

                }
            }
            return lista;
        }



        /// <summary>
        /// Metodo para leer las ordenes de compras y asociarlas a un ticket cuyo Numero OC sea identico a este
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public async Task<string> LeerExcelOC(byte[] archivo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<int> lista = new();
            string hoja = "Worksheet";

            int columna = 2;
            using (var memoryStream = new MemoryStream(archivo))
            using (ExcelPackage package = new ExcelPackage(memoryStream)) 
            {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[hoja];
                int rowCount = worksheet.Dimension.Rows;
                List<string> datosColumna = new List<string>();
                for (int row = 5; row <= rowCount; row++)
                {
                    OrdenCompra OC = new OrdenCompra();
                    OC.posicion = Convert.ToString(worksheet.Cells[row, 2].Value?.ToString());
                    OC.Numero_OC = Convert.ToInt32(worksheet.Cells[row, 3].Value?.ToString());
                    OC.Cantidad = Convert.ToInt32(worksheet.Cells[row, 4].Value?.ToString());
                    OC.Mon = Convert.ToString(worksheet.Cells[row, 6].Value?.ToString());
                    OC.PrcNeto = Convert.ToDecimal(worksheet.Cells[row, 7].Value?.ToString());
                    OC.Texto = Convert.ToString(worksheet.Cells[row, 19].Value?.ToString());
                    string fechastring = Convert.ToString(worksheet.Cells[row, 21].Value?.ToString());
                    DateTime fecha = DateTime.ParseExact(fechastring, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    DateTime fecha1 = DateTime.ParseExact(fecha.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    OC.Fecha_Recepcion = fecha1;
                    OC.Material = Convert.ToInt32(worksheet.Cells[row, 22].Value?.ToString());
                    OC.ValorNeto = Convert.ToDecimal(worksheet.Cells[row, 23].Value?.ToString());
                    OC.IsCiclica = true;
                    OC.Recepcion = false;
                    OC.Estado_OC = true;
                    OC.Id_Ticket = 0;
                    OC.Id_Orden_Compra = 0;
                    Ticket T = await IRT.GetTicketOC((int)OC.Numero_OC);
                    bool cont = true;
                    T.Estado = "Espera liberacion";
                    OC.Id_Ticket = T.ID_Ticket;
                    T= await IRT.ModificarTicket(T);
                    string Ex = await IROC.Existe((long)OC.Numero_OC,OC.posicion);
                    if(Ex == "ok")
                    {
                        OC = await IROC.NuevoOC(OC);
                    }
                }
            }
            return "listo";
        }
        }
    }

