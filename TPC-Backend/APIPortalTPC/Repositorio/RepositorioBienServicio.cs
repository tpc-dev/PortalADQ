using BaseDatosTPC;
using APIPortalTPC.Datos;
using System.Data.SqlClient;
using System.Data;
namespace APIPortalTPC.Repositorio
{
    public class RepositorioBienServicio : IRepositorioBienServicio
    {
        /// <value>variable para guardar la conexión a la base de datos</value>
        private readonly string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioBienServicio(AccesoDatos CD)
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
        /// Metodo que permite conseguir un objeto usando su llave foranea 
        /// </summary>
        /// <param name="id">Id del Bien_Servicio a buscar</param>
        /// <returns>El servicio cuyo Id sea el mismo</returns>
        /// <exception cref="Exception"></exception>
        public async Task<BienServicio> GetServicio(int id)
        {
            //Parametro para guardar el objeto a mostrar
            BienServicio bs = new();
            //Se realiza la conexion a la base de datos
            SqlConnection sql = conectar();
            //parametro que representa comando o instrucion en SQL para ejecutarse en una base de datos
            SqlCommand? Comm = null;
            //parametro para leer los resultados de una consulta
            SqlDataReader? reader = null;
            try
            {
                //Se crea la instancia con la conexion SQL para interactuar con la base de datos
                sql.Open();
                //se ejecuta la base de datos
                Comm = sql.CreateCommand();
                //se realiza la accion correspondiente en la base de datos
                //muestra los datos de la tabla correspondiente con sus condiciones
                Comm.CommandText = "SELECT * FROM dbo.Bien_Servicio where ID_Bien_Servicio = @ID_Bien_Servicio";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = id;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    bs.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    bs.Bien_Servicio = (Convert.ToString(reader["Bien_Servicio"])).Trim(); ;
                    bs.Activado = Convert.ToBoolean(reader["Activado"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Bien_Servicio " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader?.Close();
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return bs;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objetos
        /// </summary>
        /// <returns>Lista con todos los Bienes yServicios</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<BienServicio>> GetAllServicio()
        {
            List<BienServicio> lista = new List<BienServicio>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Bien_Servicio where Activado = @A"; // leer base datos 
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@A", SqlDbType.Bit).Value = true;
                reader = await Comm.ExecuteReaderAsync();
                //acontinuacion se procede a pasar los datos a una clase y luego se guardan en una lista
                while (reader.Read())
                {
                    BienServicio bs = new();
                    bs.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    bs.Bien_Servicio = (Convert.ToString(reader["Bien_Servicio"])).Trim();
                    bs.Activado = Convert.ToBoolean(reader["Activado"]);
                    lista.Add(bs);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Bien_Servicio " + ex.Message);
            }
            finally
            {
                reader?.Close();
                Comm?.Dispose();
                sql?.Close();
                sql?.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="bs">Objeto Bien_Servicio Modificado</param>
        /// <returns>Retorna el objeto BienServicio que se ha modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<BienServicio> ModificarBien_Servicio(BienServicio bs)
        {
            BienServicio? bsmodificado = null;
            SqlConnection? sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Bien_Servicio SET Bien_Servicio = @Bien_Servicio WHERE ID_Bien_Servicio = @ID_Bien_Servicio";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = bs.ID_Bien_Servicio;
                Comm.Parameters.Add("@Bien_Servicio", SqlDbType.VarChar, 50).Value = bs.Bien_Servicio;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    bsmodificado = await GetServicio(Convert.ToInt32(reader["ID_Bien_Servicio"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el bien/servicio " + ex.Message);
            }
            finally
            {
                reader?.Close();

                Comm?.Dispose();
                sqlConexion?.Close();
                sqlConexion?.Dispose();
            }
            return bsmodificado;
        }
        /// <summary>
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="bs">Objeto BienServicio a añadir</param>
        /// <returns>EL objeto BienServicio agregado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<BienServicio> NuevoBienServicio(BienServicio bs)
        {
            SqlConnection? sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Bien_Servicio (Bien_Servicio) " +
                    "VALUES (@Bien_Servicio); SELECT SCOPE_IDENTITY() AS ID_Bien_Servicio";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Bien_Servicio", SqlDbType.VarChar, 50).Value = bs.Bien_Servicio;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                int id = (int)idDecimal;
                bs.ID_Bien_Servicio = id;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Bien_Servicio " + ex.Message);
            }
            finally
            {
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return bs;
        }
        /// <summary>
        /// Metodo para evitar duplicados
        /// </summary>
        /// <param name="BienServicio"></param>
        /// <returns></returns>
        public async Task<string> Existe(string BienServicio)
        {
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Bien_Servicio WHERE Bien_Servicio = @Bien_Servicio";
                    command.Parameters.AddWithValue("@Bien_Servicio", BienServicio);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        return "El bien o servicio ya existe";
                    }
                    else
                    {
                        reader.Close();
                        return "ok";
                    }
                }
            }
        }

        /// <summary>
        /// "Elimina" el bien y servicio
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BienServicio> EliminarBien_Servicio(int bs)
        {
            BienServicio? bsmodificado = null;
            SqlConnection? sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Bien_Servicio " +
                    "SET Activado=@Activado " +
                    "WHERE ID_Bien_Servicio = @ID_Bien_Servicio";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = bs;
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = false;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    bsmodificado = await GetServicio(Convert.ToInt32(reader["ID_Bien_Servicio"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el bien/servicio " + ex.Message);
            }
            finally
            {
                reader?.Close();

                Comm?.Dispose();
                sqlConexion?.Close();
                sqlConexion?.Dispose();
            }
            return bsmodificado;
        }

        public async Task<BienServicio> GetServicioNombre(string bsn)
        {
            //Parametro para guardar el objeto a mostrar
            BienServicio bs = new();
            //Se realiza la conexion a la base de datos
            SqlConnection sql = conectar();
            //parametro que representa comando o instrucion en SQL para ejecutarse en una base de datos
            SqlCommand? Comm = null;
            //parametro para leer los resultados de una consulta
            SqlDataReader? reader = null;
            try
            {
                //Se crea la instancia con la conexion SQL para interactuar con la base de datos
                sql.Open();
                //se ejecuta la base de datos
                Comm = sql.CreateCommand();
                //se realiza la accion correspondiente en la base de datos
                //muestra los datos de la tabla correspondiente con sus condiciones
                Comm.CommandText = "SELECT * FROM dbo.Bien_Servicio where Bien_Servicio = @BS";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@BS", SqlDbType.Int).Value = bsn;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    bs.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    bs.Bien_Servicio = (Convert.ToString(reader["Bien_Servicio"])).Trim(); ;
                    bs.Activado = Convert.ToBoolean(reader["Activado"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Bien_Servicio " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader?.Close();
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return bs;
        }
    }
}
