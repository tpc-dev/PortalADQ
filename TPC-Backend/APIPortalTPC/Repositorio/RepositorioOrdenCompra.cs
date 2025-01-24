using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioOrdenCompra : IRepositorioOrdenCompra
    {
       
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioOrdenCompra(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
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
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="OC">Objeto OrdenCompra a añadir a la base de datos</param>
        /// <returns>El objeto OrdenCompra a añadir</returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenCompra> NuevoOC(OrdenCompra OC)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Orden_de_Compra " +
                    "(Numero_OC,Posicion,Id_Ticket,Texto,Cantidad,Mon,PrcNeto,Material,ValorNeto,Recepcion) " +
                    "VALUES (@Numero_OC,@Posicion,@Id_Ticket,@Texto,@Cantidad,@Mon,@PrcNeto,@Material,@ValorNeto,@Recepcion) " +
                    "SELECT SCOPE_IDENTITY() AS Id_Orden_Compra";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Numero_OC", SqlDbType.Int).Value = OC.Numero_OC;
                Comm.Parameters.Add("@Posicion", SqlDbType.VarChar, 10).Value = OC.posicion;
                Comm.Parameters.Add("@Id_Ticket", SqlDbType.Int).Value = OC.Id_Ticket;

                Comm.Parameters.Add("@Texto", SqlDbType.VarChar,500).Value = OC.Texto;

                Comm.Parameters.Add("@Cantidad", SqlDbType.Int).Value = OC.Cantidad;

                Comm.Parameters.Add("@Mon", SqlDbType.VarChar,100).Value = OC.Mon;
                Comm.Parameters.Add("@PrcNeto", SqlDbType.Float).Value = OC.PrcNeto;
                Comm.Parameters.Add("@Material", SqlDbType.Int).Value = OC.Material;
                Comm.Parameters.Add("@ValorNeto", SqlDbType.Float).Value = OC.ValorNeto;
                Comm.Parameters.Add("@Recepcion", SqlDbType.Bit).Value = OC.Recepcion;

                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                OC.Id_Orden_Compra = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Orden de compra " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return OC;
        }

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Id que pertenece al objeto OrdenCompra a buscar</param>
        /// <returns>Retorna el objeto cuya Id coincide con el pedido</returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenCompra> GetOC(int id)
        {
            //Parametro para guardar el objeto a mostrar
            OrdenCompra oc = new OrdenCompra();
            //Se realiza la conexion a la base de datos
            SqlConnection sql = conectar();
            //parametro que representa comando o instrucion en SQL para ejecutarse en una base de datos
            SqlCommand? Comm = null;
            //parametro para leer los resultados de una consulta
            SqlDataReader reader = null;
            try
            {
                //Se crea la instancia con la conexion SQL para interactuar con la base de datos
                sql.Open();
                //se ejecuta la base de datos
                Comm = sql.CreateCommand();
                //se realiza la accion correspondiente en la base de datos
                //muestra los datos de la tabla correspondiente con sus condiciones
                Comm.CommandText = @"SELECT OC.*, P.ID_Proveedores, P.Nombre_Fantasia  , T.Fecha_Creacion_OC ,T.ID_Proveedor
                FROM dbo.Orden_de_Compra OC 
				Left Outer join dbo.Ticket T ON T.ID_Ticket = OC.Id_Ticket 
                LEFT OUTER JOIN dbo.Proveedores P ON  T.ID_Proveedor  = P.ID_Proveedores 
                where OC.Id_Orden_Compra = @Id_Orden_Compra
               ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Orden_Compra", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                    oc.Id_Orden_Compra = Convert.ToInt32(reader["Id_Orden_Compra"]);
                    oc.Numero_OC = Convert.ToInt32(reader["Numero_OC"]);
                    oc.Fecha_Recepcion = Convert.ToDateTime(reader["Fecha_Creacion_OC"]);
                    oc.Id_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                    oc.Texto = Convert.ToString(reader["Texto"]).Trim();
                    oc.IsCiclica = Convert.ToBoolean(reader["IsCiclica"]);
                    oc.IdP = Convert.ToInt32(reader["ID_Proveedor"]);
                    oc.posicion = Convert.ToString(reader["Posicion"]).Trim();
                    oc.Cantidad = Convert.ToInt32(reader["Cantidad"]);
                    oc.Mon = Convert.ToString(reader["Mon"]).Trim();
                    oc.PrcNeto = Convert.ToDecimal(reader["PrcNeto"]);
                    string Prov = Convert.ToString(reader["ID_Proveedores"]);
                    Prov = Prov.Trim() + " " + Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    oc.Proveedor = Prov;
                    oc.Material = Convert.ToInt64(reader["Material"]);
                    oc.ValorNeto = Convert.ToDecimal(reader["ValorNeto"]);
                    oc.Recepcion = Convert.ToBoolean(reader["Recepcion"]);
                    oc.Estado_OC = Convert.ToBoolean(reader["Estado_OC"]);

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla OrdenCompra " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return oc;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna la lista con todos los objetos OrdenCompra</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<OrdenCompra>> GetAllOC()
        {
            List<OrdenCompra> lista = new List<OrdenCompra>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = @"SELECT OC.*, P.ID_Proveedores, P.Nombre_Fantasia  , T.Fecha_Creacion_OC ,T.ID_Proveedor
                FROM dbo.Orden_de_Compra OC 
				Left Outer join dbo.Ticket T ON T.ID_Ticket = OC.Id_Ticket 
                LEFT OUTER JOIN dbo.Proveedores P ON  T.ID_Proveedor  = P.ID_Proveedores  
                Where OC.Estado_OC = @A"
;               // leer base datos 
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@A", SqlDbType.Bit).Value = true;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    OrdenCompra oc = new();

                    oc.Id_Orden_Compra = Convert.ToInt32(reader["Id_Orden_Compra"]);
                    oc.Numero_OC = Convert.ToInt32(reader["Numero_OC"]);
                    oc.Fecha_Recepcion = Convert.ToDateTime(reader["Fecha_Creacion_OC"]);
                    oc.Id_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                    oc.Texto = Convert.ToString(reader["Texto"]).Trim();
                    oc.posicion = Convert.ToString(reader["Posicion"]).Trim();
                    oc.Cantidad = Convert.ToInt32(reader["Cantidad"]);
                    oc.IdP = Convert.ToInt32(reader["ID_Proveedor"]);
                    oc.Mon = Convert.ToString(reader["Mon"]).Trim();
                    oc.PrcNeto = Convert.ToDecimal(reader["PrcNeto"]);
                    string Prov = Convert.ToString(reader["ID_Proveedores"]);
                    Prov = Prov.Trim() + " " + Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    oc.Proveedor = Prov;
                    oc.Material = Convert.ToInt64(reader["Material"]);
                    oc.ValorNeto = Convert.ToDecimal(reader["ValorNeto"]);
                    oc.Recepcion = Convert.ToBoolean(reader["Recepcion"]);
                    oc.Estado_OC = Convert.ToBoolean(reader["Estado_OC"]);

                    lista.Add(oc);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Cotización " + ex.Message);
            }
            finally
            {
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="OC">Objetivo del tipo OrdenCompra que va a modificarse en la base de datos</param>
        /// <returns>Retorna el objeto OrdenCompra modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenCompra> ModificarOC(OrdenCompra OC)
        {
            OrdenCompra ocmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Orden_de_Compra SET " +
                                   "Numero_OC = @Numero_OC, " +  
                                   "Posicion = @Posicion, " +
                                   "Texto = @Texto, " +
                                   "Cantidad = @Cantidad, " +
                                   "Mon=@Mon, " +
                                   "PrcNeto=@PrcNeto, " +

                                   "Material= @Material, " +
                                   "ValorNeto=@ValorNeto, " +
                                   "Recepcion=@Recepcion " +
                                   "WHERE Id_Orden_Compra = @Id_OE";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Numero_OC", SqlDbType.Int).Value = OC.Numero_OC;
                Comm.Parameters.Add("@Posicion", SqlDbType.VarChar).Value = OC.posicion;
                Comm.Parameters.Add("@Texto", SqlDbType.VarChar,500).Value = OC.Texto;
                Comm.Parameters.Add("@Cantidad", SqlDbType.Int).Value = OC.Cantidad;

                Comm.Parameters.Add("@Mon", SqlDbType.VarChar, 100).Value = OC.Mon;
                Comm.Parameters.Add("@PrcNeto", SqlDbType.Float).Value = OC.PrcNeto;

                Comm.Parameters.Add("@Material", SqlDbType.BigInt).Value = OC.Material;
                Comm.Parameters.Add("@ValorNeto", SqlDbType.Float).Value = OC.ValorNeto;
                Comm.Parameters.Add("@Recepcion", SqlDbType.Bit).Value = OC.Recepcion;

                Comm.Parameters.Add("@Id_OE", SqlDbType.Int).Value = OC.Id_Orden_Compra;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    ocmod = await GetOC(Convert.ToInt32(reader["Id_OE"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la orden de compra " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return ocmod;
        }
        /// <summary>
        /// "Elimina" la OC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenCompra> EliminarOC(int id)
        {
            OrdenCompra ocmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Orden_de_Compra SET " +
                    "Estado_OC =@Estado_OC " +
                    "WHERE Id_Orden_Compra = @Id_OE";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("Estado_OC", SqlDbType.Bit).Value = false;

                Comm.Parameters.Add("@Id_OE", SqlDbType.Int).Value =id;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    ocmod = await GetOC(Convert.ToInt32(reader["Id_OE"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la orden de compra " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return ocmod;
        }

        
        /// <summary>
        /// Obtienes todos las OC de un ticket
        /// </summary>
        /// <param name="id_T"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<OrdenCompra>> GetAllOCTicket(int id_T)
        {
            List<OrdenCompra> lista = new List<OrdenCompra>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = @"SELECT OC.*, P.ID_Proveedores, P.Nombre_Fantasia  , T.Fecha_Creacion_OC ,T.ID_Proveedor
                FROM dbo.Orden_de_Compra OC 
				Left Outer join dbo.Ticket T ON T.ID_Ticket = OC.Id_Ticket 
                LEFT OUTER JOIN dbo.Proveedores P ON T.ID_Proveedor = P.ID_Proveedores
                where OC.Id_Ticket = @Ticket "
;               // leer base datos 
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@Ticket", SqlDbType.Int).Value = id_T;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    OrdenCompra oc = new();
                    oc.Id_Orden_Compra = Convert.ToInt32(reader["Id_Orden_Compra"]);
                    oc.Numero_OC = Convert.ToInt32(reader["Numero_OC"]);
                    oc.Fecha_Recepcion = Convert.ToDateTime(reader["Fecha_Creacion_OC"]);
                    oc.Id_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                    oc.Texto = Convert.ToString(reader["Texto"]).Trim();
                    oc.posicion = Convert.ToString(reader["Posicion"]).Trim();
                    oc.Cantidad = Convert.ToInt32(reader["Cantidad"]);
                    oc.IdP = Convert.ToInt32(reader["ID_Proveedor"]);
                    oc.Mon = Convert.ToString(reader["Mon"]).Trim();
                    oc.PrcNeto = Convert.ToDecimal(reader["PrcNeto"]);
                    string Prov = Convert.ToString(reader["ID_Proveedores"]);
                    Prov = Prov.Trim() + " " + Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    oc.Proveedor = Prov;
                    oc.Material = Convert.ToInt64(reader["Material"]);
                    oc.ValorNeto = Convert.ToDecimal(reader["ValorNeto"]);
                    oc.Recepcion = Convert.ToBoolean(reader["Recepcion"]);
                    oc.Estado_OC = Convert.ToBoolean(reader["Estado_OC"]);

                    lista.Add(oc);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Cotización " + ex.Message);
            }
            finally
            {
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Metodo que se asegura de que una OC sea unica, para ser unica, su Numero_OC y posicion NO deben repetirse
        /// </summary>
        /// <param name="Numero_OC"></param>
        /// <param name="posicion"></param>
        /// <returns></returns>
        public async Task<string> Existe(long Numero_OC, string posicion)
        {
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Orden_de_Compra WHERE Numero_OC = @OC and Posicion =@OP";
                    command.Parameters.AddWithValue("@OC", Numero_OC);
                    command.Parameters.AddWithValue("@OP", posicion);
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        reader.Close();
                        return "La posicion ya existe";
                    }
                    else
                    {
                        
                        reader.Close();
                        return "ok";
                    }
                }
            }

        }
    }
}
