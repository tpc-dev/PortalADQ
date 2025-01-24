using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioOrdenesEstadisticas : IRepositorioOrdenesEstadisticas
    {

        private string Conexion;
        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioOrdenesEstadisticas(AccesoDatos CD)
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
        /// <param name="OE">Objeto Ordenes_Estadisticas que se agregara a la base de datos</param>
        /// <returns>Retorna el objeto creado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenesEstadisticas> NuevoOE(OrdenesEstadisticas OE)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Ordenes_estadisticas " +
                    "(Nombre,Codigo_OE,Id_Centro_de_Costo,Activado) " +
                    "VALUES (@Nombre,@Codigo_OE,@Id_Centro_de_Costo,@Activado); " +
                    "SELECT SCOPE_IDENTITY() AS Id_Orden_estadistica";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = OE.Nombre;
                Comm.Parameters.Add("@Codigo_OE", SqlDbType.VarChar, 50).Value = OE.Codigo_OE;
                Comm.Parameters.Add("@Id_Centro_de_Costo", SqlDbType.Int).Value = OE.Id_Centro_de_Costo;
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = OE.Activado;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                OE.Id_Orden_Estadistica = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Orden Estadsitica " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return OE;
        }

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Id del objeto </param>
        /// <returns>Retorna el objeto Ordenes_Estadisticas con la Id pedida</returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenesEstadisticas> GetOE(int id)
        {
            //Parametro para guardar el objeto a mostrar
            OrdenesEstadisticas OE = new();
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
                Comm.CommandText = "SELECT OE.*,CeCo.NombreCeCo,CeCo.Id_Ceco, CeCo.Id_Ceco " +
                    "FROM dbo.Ordenes_estadisticas OE " +
                    "Inner join dbo.Centro_de_costo CeCo ON OE.Id_Centro_de_Costo = CeCo.Id_Ceco " +
                    "where OE.Id_Orden_Estadistica = @Id_Orden_Estadistica ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Orden_Estadistica", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    OE.Nombre = (Convert.ToString(reader["Nombre"])).Trim();
                    OE.Codigo_OE = (Convert.ToString(reader["Codigo_OE"])).Trim();
                    OE.Id_Centro_de_Costo = Convert.ToString(reader["NombreCeCo"]).Trim();
                    OE.Id_Orden_Estadistica = Convert.ToInt32(reader["Id_Orden_Estadistica"]);
                    OE.Activado = Convert.ToBoolean(reader["Activado"]);
                    OE.Id_CeCo = Convert.ToInt32(reader["Id_Ceco"]);

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Cotizaciones " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return OE;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos Ordenes_Estadisticas</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<OrdenesEstadisticas>> GetAllOE()
        {
            List<OrdenesEstadisticas> lista = new List<OrdenesEstadisticas>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT OE.*,CeCo.NombreCeCo, CeCo.Id_Ceco " +
                    "FROM dbo.Ordenes_estadisticas OE " +
                    "Inner join dbo.Centro_de_costo CeCo ON OE.Id_Centro_de_Costo = CeCo.Id_Ceco " +
                    "where OE.Activado = @A"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@A", SqlDbType.Bit).Value = true;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    OrdenesEstadisticas OE = new();
                    OE.Nombre = (Convert.ToString(reader["Nombre"])).Trim();
                    OE.Codigo_OE = (Convert.ToString(reader["Codigo_OE"])).Trim();
                    OE.Id_Centro_de_Costo = Convert.ToString(reader["NombreCeCo"]).Trim();
                    OE.Id_Orden_Estadistica = Convert.ToInt32(reader["Id_Orden_Estadistica"]);
                    OE.Activado = Convert.ToBoolean(reader["Activado"]);
                    OE.Id_CeCo = Convert.ToInt32(reader["Id_Ceco"]);
          
                    lista.Add(OE);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Ordenes Estadisticas " + ex.Message);
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
        /// <param name="OE">Objeto Ordenes_Estadisticas que se va a modificar</param>
        /// <returns>Retorna el objeto a modificar</returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenesEstadisticas> ModificarOE(OrdenesEstadisticas OE)
        {
            OrdenesEstadisticas OEmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Ordenes_Estadisticas SET " +
                    "Nombre = @Nombre, " +
                    "Codigo_OE = @Codigo_OE, " +
                    "Id_Centro_de_Costo = @Id_Centro_de_Costo, " +
                    "Activado = @A " +
                    "WHERE Id_Orden_Estadistica = @Id_Orden_Estadistica";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Orden_Estadistica", SqlDbType.Int).Value = OE.Id_Orden_Estadistica;
                Comm.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = OE.Nombre;
                Comm.Parameters.Add("@Codigo_OE", SqlDbType.VarChar, 50).Value = OE.Codigo_OE;
                Comm.Parameters.Add("@Id_Centro_de_Costo", SqlDbType.Int).Value = Int32.Parse(OE.Id_Centro_de_Costo) ;
                Comm.Parameters.Add("@A", SqlDbType.Bit).Value = true;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    OEmod = await GetOE(Convert.ToInt32(reader["Id_Orden_Estadistica"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la orden estadistica!!! " + ex.Message+ " "+OE.Id_Orden_Estadistica);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return OEmod;
        }
        /// <summary>
        /// metodo para asegurarse que no se dupliquen los datos
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> Existe(string code)
        {
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Ordenes_estadisticas WHERE Codigo_OE = @code";
                    command.Parameters.AddWithValue("@code", code);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        return "El codigo ya existe";
                    }
                    else
                    {
                        reader.Close();
                        return "ok";
                    }
                }
            }
        }

        public async Task<OrdenesEstadisticas> EliminarOE(int OE)
        {
            OrdenesEstadisticas OEmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Ordenes_Estadisticas SET " +
                    "Activado = @Activado " +
                    "WHERE Id_Orden_Estadistica = @Id_Orden_Estadistica";
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@Id_Orden_Estadistica", SqlDbType.Int).Value = OE;
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = false;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    OEmod = await GetOE(Convert.ToInt32(reader["Id_Orden_Estadistica"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error eliminando la orden estadistica " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return OEmod;
        }
        /// <summary>
        /// Metodo que obtienes una Orden Estadistica por su Codigo, no por su iD
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenesEstadisticas> GetOECode(string code)
        {
            //Parametro para guardar el objeto a mostrar
            OrdenesEstadisticas OE = new();
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
                Comm.CommandText = "SELECT OE.*,CeCo.NombreCeCo " +
                    "FROM dbo.Ordenes_estadisticas OE " +
                    "Inner join dbo.Centro_de_costo CeCo ON OE.Id_Centro_de_Costo = CeCo.Id_Ceco " +

                    "where OE.Codigo_OE = @Codigo_OE ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Codigo_OE", SqlDbType.VarChar).Value = code;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    OE.Nombre = (Convert.ToString(reader["Nombre"])).Trim();
                    OE.Codigo_OE = (Convert.ToString(reader["Codigo_OE"])).Trim();
                    OE.Id_Centro_de_Costo = Convert.ToString(reader["NombreCeCo"]).Trim();
                    OE.Id_Orden_Estadistica = Convert.ToInt32(reader["Id_Orden_Estadistica"]);
                    OE.Activado = Convert.ToBoolean(reader["Activado"]);
                    OE.Id_CeCo = Convert.ToInt32(reader["Id_Centro_de_Costo"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Cotizaciones " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return OE;
        }
    }
}