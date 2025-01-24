using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;
using NPOI.SS.Formula.Functions;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioTicket : IRepositorioTicket
    {

        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioTicket(AccesoDatos CD)
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
        /// <param name="T">Objeto del tipo Ticket que se va a agregar a la base de datos</param>
        /// <returns>Retorna el objeto a modificar</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Ticket> NewTicket(Ticket T)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Ticket " +
                                         "(Estado,Fecha_Creacion_OC,Id_Usuario,ID_Proveedor, Fecha_OC_Recepcionada, Fecha_OC_Enviada, Fecha_OC_Liberada, Detalle, Solped, Id_OE) " +
                                         "VALUES (@Estado,@Fecha_Creacion_OC,@Id_Usuario,@ID_Proveedor, @Fecha_OC_Recepcionada, @Fecha_OC_Enviada, @Fecha_OC_Liberada, @Detalle, @Solped, @Id_OE); " +
                                         "SELECT SCOPE_IDENTITY() AS ID_Ticket;";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = T.Estado;
                Comm.Parameters.Add("@Fecha_Creacion_OC", SqlDbType.DateTime).Value = DateTime.Now;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = T.Id_Usuario;
                Comm.Parameters.Add("@Id_Proveedor", SqlDbType.Int).Value = T.ID_Proveedor;
                Comm.Parameters.Add("@Fecha_OC_Recepcionada", SqlDbType.DateTime).Value = DBNull.Value;
                Comm.Parameters.Add("@Fecha_OC_Enviada", SqlDbType.DateTime).Value = DBNull.Value;
                Comm.Parameters.Add("@Fecha_OC_Liberada", SqlDbType.DateTime).Value = DBNull.Value;

                Comm.Parameters.Add("@Detalle", SqlDbType.VarChar, 50).Value = T.Detalle;

                if (T.Solped != null)
                    Comm.Parameters.Add("@Solped", SqlDbType.BigInt).Value = T.Solped;
                else Comm.Parameters.Add("@Solped", SqlDbType.BigInt).Value = DBNull.Value;
                Comm.Parameters.Add("@Id_OE", SqlDbType.Int).Value = T.Id_OE;

                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                T.ID_Ticket = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Ticket " + ex.Message);

            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return T;
        }

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Id del objeto Ticket a buscar</param>
        /// <returns>Retorna el objeto Ticket que se busca</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Ticket> GetTicket(int id)
        {
            //Parametro para guardar el objeto a mostrar
            List<Ticket> lista = new List<Ticket>();
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
                Comm.CommandText =
                    "SELECT T.*,U.Nombre_Usuario , p.Nombre_Fantasia, OE.Nombre, OC.Numero_OC " +
                    "FROM dbo.Ticket T " +
                    "INNER JOIN dbo.Usuario U on U.Id_Usuario = T.Id_Usuario " +
                    "INNER JOIN dbo.Proveedores p ON T.ID_Proveedor = p.ID_Proveedores " +
                    "Left JOIN dbo.Orden_de_Compra OC ON T.ID_Ticket = OC.Id_Ticket " +
                    "LEFT JOIN dbo.Ordenes_Estadisticas OE  On OE.Id_Orden_Estadistica = T.Id_OE " +
                    "where T.ID_Ticket = @ID_Ticket";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID_Ticket", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Ticket T = new();
                    T.Estado = Convert.ToString(reader["Estado"]).Trim();
                    T.Fecha_Creacion_OC = (DateTime)reader["Fecha_Creacion_OC"];
                    T.Id_Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    T.ID_Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    T.Fecha_OC_Recepcionada = reader["Fecha_OC_Recepcionada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Recepcionada"];
                    T.Fecha_OC_Enviada = reader["Fecha_OC_Enviada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Enviada"];
                    T.Fecha_OC_Liberada = reader["Fecha_OC_Liberada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Liberada"];
                    T.Detalle = Convert.ToString(reader["Detalle"]).Trim();
                    T.Numero_OC = Convert.ToInt64(reader["N_OC"]);
                    T.Solped = reader.IsDBNull(reader.GetOrdinal("Solped")) ? 0 : (long)reader["Solped"];
                    T.Id_OE = Convert.ToString(reader["Nombre"]).Trim();
                    T.N_OE = Convert.ToInt32(reader["Id_OE"]);
                    T.Activado = Convert.ToBoolean(reader["Activado"]);
                    T.ID_Ticket = Convert.ToInt32(reader["ID_Ticket"]);
                    T.Id_U = Convert.ToInt32(reader["Id_Usuario"]);

                    int cont = 0;
                    foreach (Ticket ticket in lista)
                        if (ticket.ID_Ticket == T.ID_Ticket)
                            cont = 1;

                    if (cont == 0)
                    {
                        lista.Add(T);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return lista[0];
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna lista con todos los objetos Ticket de la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Ticket>> GetAllTicket()
        {
            List<Ticket> lista = new List<Ticket>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT T.*,U.Nombre_Usuario , p.Nombre_Fantasia, OE.Nombre, OC.Numero_OC " +
                    "FROM dbo.Ticket T " +
                    "INNER JOIN dbo.Usuario U on U.Id_Usuario = T.Id_Usuario " +
                    "INNER JOIN dbo.Proveedores p ON T.ID_Proveedor = p.ID_Proveedores " +
                    "Left JOIN dbo.Orden_de_Compra OC ON T.ID_Ticket = OC.Id_Ticket " +
                    "LEFT JOIN dbo.Ordenes_Estadisticas OE  On OE.Id_Orden_Estadistica = T.Id_OE "; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Ticket T = new();
                    T.Estado = Convert.ToString(reader["Estado"]).Trim();
                    T.Fecha_Creacion_OC = (DateTime)reader["Fecha_Creacion_OC"];
                    T.Id_Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    T.ID_Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    T.Fecha_OC_Recepcionada = reader["Fecha_OC_Recepcionada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Recepcionada"];
                    T.Fecha_OC_Enviada = reader["Fecha_OC_Enviada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Enviada"];
                    T.Fecha_OC_Liberada = reader["Fecha_OC_Liberada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Liberada"];
                    T.Detalle = Convert.ToString(reader["Detalle"]).Trim();
                    T.Numero_OC = Convert.ToInt64(reader["N_OC"]);
                    T.Solped = reader.IsDBNull(reader.GetOrdinal("Solped")) ? 0 : (long)reader["Solped"];
                    T.Id_OE = Convert.ToString(reader["Nombre"]).Trim();
                    T.Activado = Convert.ToBoolean(reader["Activado"]);
                    T.ID_Ticket = Convert.ToInt32(reader["ID_Ticket"]);
                    T.Id_U = Convert.ToInt32(reader["Id_Usuario"]);
                    T.N_OE = Convert.ToInt32(reader["Id_OE"]);

                    int cont = 0;
                    foreach (Ticket ticket in lista)
                        if (ticket.ID_Ticket == T.ID_Ticket)
                            cont = 1;

                    if (cont == 0)
                    {
                        lista.Add(T);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
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
        /// <param name="T">Objeto del tipo Ticket que se quiere modificar</param>
        /// <returns>Retorna el objeto modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Ticket> ModificarTicket(Ticket T)
        {
            Ticket Tmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;

            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Ticket SET " +
                    "Estado = ISNULL(@Estado, Estado)," +
                    "Solped = @Solped, " +
                    "Fecha_Creacion_OC = @Fecha_Creacion_OC, " +
                    "Fecha_OC_Recepcionada = ISNULL(@Fecha_OC_Recepcionada, Fecha_OC_Recepcionada),  " +
                    "Fecha_OC_Enviada = ISNULL(@Fecha_OC_Enviada, Fecha_OC_Enviada), " +
                    "Fecha_OC_Liberada = ISNULL(@Fecha_OC_Liberada, Fecha_OC_Liberada), " +
                    "Detalle = @Detalle," +
                    "Id_OE = @Id_OE, " +
                    "N_OC = @N_OC " +
                    "WHERE ID_Ticket = @ID_Ticket";
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = T.Estado;
                Comm.Parameters.Add("@Fecha_Creacion_OC", SqlDbType.DateTime).Value = T.Fecha_Creacion_OC;

                if (T.Fecha_OC_Recepcionada.HasValue)
                    Comm.Parameters.Add("@Fecha_OC_Recepcionada", SqlDbType.DateTime).Value = T.Fecha_OC_Recepcionada;
                else
                    Comm.Parameters.Add("@Fecha_OC_Recepcionada", SqlDbType.DateTime).Value = DBNull.Value;
                if (T.Fecha_OC_Enviada.HasValue)
                    Comm.Parameters.Add("@Fecha_OC_Enviada", SqlDbType.DateTime).Value = T.Fecha_OC_Enviada;
                else
                    Comm.Parameters.Add("@Fecha_OC_Enviada", SqlDbType.DateTime).Value = DBNull.Value;
                if (T.Fecha_OC_Liberada.HasValue)
                    Comm.Parameters.Add("@Fecha_OC_Liberada", SqlDbType.DateTime).Value = T.Fecha_OC_Liberada;
                else
                    Comm.Parameters.Add("@Fecha_OC_Liberada", SqlDbType.DateTime).Value = DBNull.Value;

                Comm.Parameters.Add("@Detalle", SqlDbType.VarChar, 50).Value = T.Detalle;

                Comm.Parameters.Add("@Solped", SqlDbType.BigInt).Value = T.Solped;

                Comm.Parameters.Add("@Id_OE", SqlDbType.Int).Value = T.N_OE;

                Comm.Parameters.Add("@N_OC", SqlDbType.BigInt).Value = T.Numero_OC;

                Comm.Parameters.Add("@ID_Ticket", SqlDbType.Int).Value = T.ID_Ticket;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Tmod = await GetTicket(Convert.ToInt32(reader["ID_Ticket"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el Ticket " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Tmod;
        }


        /// <summary>
        /// Metodo que actualiza el estado del ticket dependiendo de su cotizaciones
        /// </summary>
        /// <param name="id_T"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Ticket> ActualizarEstadoTicket(int id_T)
        {
            Ticket T = await GetTicket(id_T);
            List<OrdenCompra> lista = new();
            //Se realiza la conexion a la base de datos
            SqlConnection sql = conectar();
            //parametro que representa comando o instrucion en SQL para ejecutarse en una base de datos
            SqlCommand? Comm = null;
            //parametro para leer los resultados de una consulta
            SqlDataReader reader = null;
            int total = 0;
            int cont = 0;
            try
            {
                //Se crea la instancia con la conexion SQL para interactuar con la base de datos
                sql.Open();
                //se ejecuta la base de datos
                Comm = sql.CreateCommand();
                //se realiza la accion correspondiente en la base de datos
                //muestra los datos de la tabla correspondiente con sus condiciones
                Comm.CommandText = @"SELECT OC.* 
                FROM dbo.Orden_de_Compra OC 
                where Oc.Id_Ticket = @Id_Ticket
               ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Ticket", SqlDbType.Int).Value = T.ID_Ticket;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    bool recep = Convert.ToBoolean(reader["Recepcion"]);
                    bool activada = Convert.ToBoolean(reader["Estado_OC"]);
                    if (activada)
                    {
                        if (recep)
                            cont++;
                        total++;
                    }

                }
                if (cont == 0)
                    T.Estado = "OC Enviada";

                else if (cont == total)
                    T.Estado = "OC Recepcionada";

                else
                    T.Estado = "OC Parcial";

                reader.Close();
                Comm.Dispose();
                Comm.CommandText = "UPDATE dbo.Ticket SET " +
                    "Estado = ISNULL(@Estado, Estado) " +
                    "WHERE ID_Ticket = @ID_Ticket";
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = T.Estado;
                reader = await Comm.ExecuteReaderAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
            }
            finally
            {

                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return T;
        }
        /// <summary>
        /// Metodo para "eliminar" el ticket, cambia el valor de Activado a false y las cotizaciones asociadas tambien
        /// </summary>
        /// <param name="id_T"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Ticket> EliminarTicket(int id_T)
        {
            Ticket T = await GetTicket(id_T);
            //Se realiza la conexion a la base de datos
            SqlConnection sql = conectar();
            //parametro que representa comando o instrucion en SQL para ejecutarse en una base de datos
            SqlCommand? Comm = null;
            //parametro para leer los resultados de una consulta
            SqlDataReader reader = null;
            int cont = 0;
            int total = 0;
            try
            {

                //Se crea la instancia con la conexion SQL para interactuar con la base de datos
                sql.Open();
                //se ejecuta la base de datos
                Comm = sql.CreateCommand();
                //se realiza la accion correspondiente en la base de datos
                //muestra los datos de la tabla correspondiente con sus condiciones
                Comm.CommandText = "UPDATE dbo.Ticket SET " +
                    "Estado = ISNULL(@Estado, Estado)," +
                    "Activado = @Activado " +
                    "WHERE ID_Ticket = @ID_Ticket";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = false;
                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = "OC Cancelada";
                Comm.Parameters.Add("@ID_Ticket", SqlDbType.Int).Value = id_T;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    T = await GetTicket(id_T);
                //permite regresar objetos de la base de datos para que se puedan leer

            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return T;
        }

        /// <summary>
        /// Obtienes todos los ticked de un Usuario por su Id_Usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Ticket>> GetAllTicketUsuario(int id)
        {

            //Parametro para guardar el objeto a mostrar
            List<Ticket> lista = new List<Ticket>();
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
                Comm.CommandText =
                    "SELECT T.*,U.Nombre_Usuario , p.Nombre_Fantasia, OE.Nombre, OC.Numero_OC " +
                    "FROM dbo.Ticket T " +
                    "INNER JOIN dbo.Usuario U on U.Id_Usuario = T.Id_Usuario " +
                    "INNER JOIN dbo.Proveedores p ON T.ID_Proveedor = p.ID_Proveedores " +
                    "Left JOIN dbo.Orden_de_Compra OC ON T.ID_Ticket = OC.Id_Ticket " +
                    "LEFT JOIN dbo.Ordenes_Estadisticas OE  On OE.Id_Orden_Estadistica = T.Id_OE " +
                    "where T.Id_Usuario = @ID";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Ticket T = new();
                    T.Estado = Convert.ToString(reader["Estado"]).Trim();
                    T.Fecha_Creacion_OC = (DateTime)reader["Fecha_Creacion_OC"];
                    T.Id_Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    T.ID_Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    T.Fecha_OC_Recepcionada = reader["Fecha_OC_Recepcionada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Recepcionada"];
                    T.Fecha_OC_Enviada = reader["Fecha_OC_Enviada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Enviada"];
                    T.Fecha_OC_Liberada = reader["Fecha_OC_Liberada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Liberada"];
                    T.Detalle = Convert.ToString(reader["Detalle"]).Trim();
                    T.Numero_OC = reader.IsDBNull(reader.GetOrdinal("Numero_OC")) ? 0 : Convert.ToInt64(reader["Numero_OC"]);
                    T.Solped = reader.IsDBNull(reader.GetOrdinal("Solped")) ? 0 : (long)reader["Solped"];
                    T.Id_OE = Convert.ToString(reader["Nombre"]).Trim();
                    T.Activado = Convert.ToBoolean(reader["Activado"]);
                    T.ID_Ticket = Convert.ToInt32(reader["ID_Ticket"]);
                    T.Id_U = Convert.ToInt32(reader["Id_Usuario"]);

                    int cont = 0;
                    foreach (Ticket ticket in lista)
                        if (ticket.ID_Ticket == T.ID_Ticket)
                            cont = 1;

                    if (cont == 0)
                    {
                        lista.Add(T);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Obtienes todos los id_tickets que tengan al menos una OC pendiente
        /// </summary>
        /// <param name="id_U"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<int>> TicketConOCPendientes(int id_U)
        {
            List<int> lista = new List<int>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = @"SELECT T.*,U.Nombre_Usuario, OC.Numero_OC  , p.Nombre_Fantasia, OE.Nombre
                FROM dbo.Ticket T
                INNER JOIN dbo.Usuario U on U.Id_Usuario = T.Id_Usuario 
                INNER JOIN dbo.Proveedores p ON T.ID_Proveedor = p.ID_Proveedores 
                Left JOIN dbo.Orden_de_Compra OC ON T.ID_Ticket = OC.Id_Ticket 
                LEFT JOIN dbo.Ordenes_Estadisticas OE  On OE.Id_Orden_Estadistica = T.Id_OE 
                where T.Id_Usuario = @Id and OC.Estado_OC = @Estado and OC.Recepcion = @activado "; // leer base datos 
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id", SqlDbType.Int).Value = id_U;
                Comm.Parameters.Add("@Estado", SqlDbType.Bit).Value = true;
                Comm.Parameters.Add("@activado", SqlDbType.Bit).Value = false;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Ticket T = new();
                    T.Estado = Convert.ToString(reader["Estado"]).Trim();
                    T.Fecha_Creacion_OC = (DateTime)reader["Fecha_Creacion_OC"];
                    T.Id_Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    T.ID_Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    T.Fecha_OC_Recepcionada = reader["Fecha_OC_Recepcionada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Recepcionada"];
                    T.Fecha_OC_Enviada = reader["Fecha_OC_Enviada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Enviada"];
                    T.Fecha_OC_Liberada = reader["Fecha_OC_Liberada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Liberada"];
                    T.Detalle = Convert.ToString(reader["Detalle"]).Trim();
                    T.Numero_OC = reader.IsDBNull(reader.GetOrdinal("Numero_OC")) ? 0 : Convert.ToInt64(reader["Numero_OC"]);
                    T.Solped = reader.IsDBNull(reader.GetOrdinal("Solped")) ? 0 : (long)reader["Solped"];
                    T.Id_OE = Convert.ToString(reader["Nombre"]).Trim();
                    T.Activado = Convert.ToBoolean(reader["Activado"]);
                    T.ID_Ticket = Convert.ToInt32(reader["ID_Ticket"]);
                    T.Id_U = Convert.ToInt32(reader["Id_Usuario"]);
                   if(!lista.Contains(T.ID_Ticket))
                        lista.Add(T.ID_Ticket);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
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
        /// Metodo que permite un ticket mediande el numero de OC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Ticket> GetTicketOC(int id)
        {
            //Parametro para guardar el objeto a mostrar
            List<Ticket> lista = new List<Ticket>();
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
                Comm.CommandText =
                    "SELECT T.*,U.Nombre_Usuario , p.Nombre_Fantasia, OE.Nombre, OC.Numero_OC " +
                    "FROM dbo.Ticket T " +
                    "INNER JOIN dbo.Usuario U on U.Id_Usuario = T.Id_Usuario " +
                    "INNER JOIN dbo.Proveedores p ON T.ID_Proveedor = p.ID_Proveedores " +
                    "Left JOIN dbo.Orden_de_Compra OC ON T.ID_Ticket = OC.Id_Ticket " +
                    "LEFT JOIN dbo.Ordenes_Estadisticas OE  On OE.Id_Orden_Estadistica = T.Id_OE " +
                    "where T.N_OC = @OC";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@OC", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Ticket T = new();
                    T.Estado = Convert.ToString(reader["Estado"]).Trim();
                    T.Fecha_Creacion_OC = (DateTime)reader["Fecha_Creacion_OC"];
                    T.Id_Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    T.ID_Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    T.Fecha_OC_Recepcionada = reader["Fecha_OC_Recepcionada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Recepcionada"];
                    T.Fecha_OC_Enviada = reader["Fecha_OC_Enviada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Enviada"];
                    T.Fecha_OC_Liberada = reader["Fecha_OC_Liberada"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_OC_Liberada"];
                    T.Detalle = Convert.ToString(reader["Detalle"]).Trim();
                    T.Numero_OC = Convert.ToInt64(reader["N_OC"]);
                    T.Solped = reader.IsDBNull(reader.GetOrdinal("Solped")) ? 0 : (long)reader["Solped"];
                    T.Id_OE = Convert.ToString(reader["Nombre"]).Trim();
                    T.N_OE = Convert.ToInt32(reader["Id_OE"]);
                    T.Activado = Convert.ToBoolean(reader["Activado"]);
                    T.ID_Ticket = Convert.ToInt32(reader["ID_Ticket"]);
                    T.Id_U = Convert.ToInt32(reader["Id_Usuario"]);

                    int cont = 0;
                    foreach (Ticket ticket in lista)
                        if (ticket.ID_Ticket == T.ID_Ticket)
                            cont = 1;

                    if (cont == 0)
                    {
                        lista.Add(T);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return lista[0];
        }


    }
}