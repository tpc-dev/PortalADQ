using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioDepartamento : IRepositorioDepartamento
    {
       
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioDepartamento(AccesoDatos CD)
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
        /// <param name="id">Id del departamento a buscar</param>
        /// <returns>retorna el objeto Departamento</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Departamento> GetDepartamento(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Departamento dep = new Departamento();
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
                Comm.CommandText = "SELECT D.*, U.Nombre_Usuario " +
                    "FROM dbo.Departamento D " +
                    "INNER JOIN dbo.Usuario U ON D.Encargado = U.Id_Usuario " +
                    "where Id_Departamento = @Id_Departamento";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Departamento", SqlDbType.Int).Value = id;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    dep.Descripcion = (Convert.ToString(reader["Descripcion"])).Trim();
                    dep.Encargado = (Convert.ToString(reader["Nombre_Usuario"])).Trim();
                    dep.Nombre = (Convert.ToString(reader["Nombre"])).Trim();
                    dep.Id_Departamento = Convert.ToInt32(reader["Id_Departamento"]);
                    dep.Id_Encargado = Convert.ToInt32(reader["Encargado"]);
                    dep.Activado = Convert.ToBoolean(reader["Activado"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Departamento " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return dep;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna lista de todos los departamentos de la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Departamento>> GetAllDepartamento()
        {
            List<Departamento> lista = new List<Departamento>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT D.*, U.Nombre_Usuario " +
                    "FROM dbo.Departamento D " +
                    "INNER JOIN dbo.Usuario U ON D.Encargado = U.Id_Usuario " +
                    "where D.Activado = @A"; // leer base datos"; 
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@A", SqlDbType.Bit).Value = true;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Departamento dep = new Departamento();
                    dep.Descripcion = (Convert.ToString(reader["Descripcion"])).Trim();
                    dep.Encargado = (Convert.ToString(reader["Nombre_Usuario"])).Trim();
                    dep.Nombre = (Convert.ToString(reader["Nombre"])).Trim();
                    dep.Id_Departamento = Convert.ToInt32(reader["Id_Departamento"]);
                    dep.Id_Encargado = Convert.ToInt32(reader["Encargado"]);
                    dep.Activado = Convert.ToBoolean(reader["Activado"]);
                    lista.Add(dep);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Departamento " + ex.Message);
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
        /// <param name="D">Objeto clase del tipo Departamento que reemplaza su homonimo por id en la base</param>
        /// <returns>Retorna el objeto Departamento modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Departamento> ModificarDepartamento(Departamento D)
        {
            Departamento Dmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Departamento SET " +
                    "Descripcion = @Descripcion, " +
                    "Encargado = @Encargado, " +
                    "Nombre = @Nombre " +
                    "WHERE Id_Departamento = @Id_Departamento";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Descripcion", SqlDbType.VarChar,50).Value = D.Descripcion;
                Comm.Parameters.Add("@Encargado", SqlDbType.Int).Value = D.Encargado;
                Comm.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = D.Nombre;
                Comm.Parameters.Add("@Id_Departamento", SqlDbType.Int).Value = D.Id_Departamento;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Dmod = await GetDepartamento(Convert.ToInt32(reader["Id_Departamento"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el departamento " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Dmod;
        }
        /// <summary>
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="D">Objeto departamento que se va a ingresar a la base de datos</param>
        /// <returns>Retorna el objeto insertado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Departamento> NuevoDepartamento(Departamento D)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO " +
                    "Departamento (Descripcion,Encargado,Nombre) " +
                    "VALUES (@Descripcion,@Encargado,@Nombre); " +
                    "SELECT SCOPE_IDENTITY() AS Id_Departamento";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Descripcion", SqlDbType.VarChar).Value = D.Descripcion;
                Comm.Parameters.Add("@Encargado", SqlDbType.VarChar, 50).Value = D.Encargado;
                Comm.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = D.Nombre;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                D.Id_Departamento = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Departamento " + ex.Message);
            }
            finally
            {
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return D;
        }
        /// <summary>
        /// Metodo que se usa para asegura que no se repita el Departamento
        /// </summary>
        /// <param name="nombre">Nombre a buscar</param>
        /// <returns></returns>
        public async Task<string> Existe(string nombre)
        {
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Departamento WHERE Nombre = @nombre";
                    command.Parameters.AddWithValue("@nombre", nombre);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        return "El nombre ya existe";
                    }
                    else
                    {
                        reader.Close();
                        return "ok";
                    }
                }
            }
        }

        public async Task<Departamento> EliminarDepartamento(int D)
        {
            Departamento Dmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Departamento SET " +
                    "Activado = @Activado " +
                    "WHERE Id_Departamento = @Id_Departamento";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = false;
                Comm.Parameters.Add("@Id_Departamento", SqlDbType.Int).Value = D;
                
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Dmod = await GetDepartamento(Convert.ToInt32(reader["Id_Departamento"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el departamento " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Dmod;
        }
    }
}