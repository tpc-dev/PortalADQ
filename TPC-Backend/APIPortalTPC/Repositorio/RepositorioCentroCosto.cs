using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioCentroCosto : IRepositorioCentroCosto
    {

        private readonly string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioCentroCosto(AccesoDatos CD)
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
        /// <param name="IdCECO">Id a buscar para el Centro Costo</param>
        /// <returns>Retorna el objeto Centro_de_costo cuyo Id sea el dado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<CentroCosto> GetCeCo(int IdCECO)
        {
            //Parametro para guardar el objeto a mostrar
            CentroCosto cc = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Centro_de_costo where Id_Ceco = @Id_Ceco";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Ceco", SqlDbType.Int).Value = IdCECO;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    cc.Id_Ceco = Convert.ToInt32(reader["Id_CeCo"]);
                    cc.Nombre = (Convert.ToString(reader["NombreCeCo"])).Trim();
                    cc.Codigo_Ceco = (Convert.ToString(reader["Codigo_Ceco"])).Trim();
                    cc.Activado = Convert.ToBoolean(reader["Activado"]);

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Centro de costo " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return cc;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objetos
        /// </summary>
        /// <returns>Retorna una lista con todos los Centro de Costo</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<CentroCosto>> GetAllCeCo()
        {
            List<CentroCosto> lista = new List<CentroCosto>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Centro_de_costo where Activado = @A"; // leer base datos 
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@A", SqlDbType.Bit).Value = true;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    CentroCosto cc = new();
                    cc.Id_Ceco = Convert.ToInt32(reader["Id_Ceco"]);
                    cc.Nombre = (Convert.ToString(reader["NombreCeCo"])).Trim();
                    cc.Codigo_Ceco = (Convert.ToString(reader["Codigo_Ceco"])).Trim();
                    cc.Activado = Convert.ToBoolean(reader["Activado"]);
                    lista.Add(cc);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Centro de costo " + ex.Message);
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
        /// <param name="CeCo">Objeto del tipo Centro_de_Costo que se usará para reemplazar el Centro_de_costo antiguo</param>
        /// <returns>Regresa el centro_de_costo que va a reemplazar</returns>
        /// <exception cref="Exception"></exception>
        public async Task<CentroCosto> ModificarCeCo(CentroCosto CeCo)
        {
            CentroCosto ccmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Centro_de_costo SET " +
                    "NombreCeCo = @NombreCeCo, " +
                     "Activado =@Activado, " +
                    "Codigo_Ceco = @Codigo_Ceco " +
                    "WHERE Id_Ceco = @Id_Ceco";
                Comm.CommandType = CommandType.Text;

                //Usar cuando se corrija el ingresar datos, porque por ahora no se como meter una clase
                Comm.Parameters.Add("@NombreCeCo", SqlDbType.VarChar, 50).Value = CeCo.Nombre;
                Comm.Parameters.Add("@Codigo_Ceco", SqlDbType.VarChar, 50).Value = CeCo.Codigo_Ceco;
                Comm.Parameters.Add("@Id_Ceco", SqlDbType.Int).Value = CeCo.Id_Ceco;
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = CeCo.Activado;
                //Comm.Parameters.Add("@Bien_Servicio", SqlDbType.VarChar, 50).Value = "Pan";
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    ccmod = await GetCeCo(Convert.ToInt32(reader["Id_Ceco"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el Centro de costo " + ex.Message);
            }
            finally
            {
                reader?.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return ccmod;
        }
        /// <summary>
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="Ceco">Objeto del tipo Centro_de_costo que se va a añadir a la base de datos</param>
        /// <returns>Regresa el objeto a añadirse</returns>
        /// <exception cref="Exception"></exception>
        public async Task<CentroCosto> Nuevo_CeCo(CentroCosto Ceco)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Centro_de_costo (NombreCeCo,Codigo_Ceco) " +
                    "VALUES (@NombreCeCo,@Codigo_Ceco); " +
                    "SELECT SCOPE_IDENTITY() AS Id_Ceco";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@NombreCeCo", SqlDbType.VarChar, 50).Value = Ceco.Nombre;
                Comm.Parameters.Add("@Codigo_Ceco", SqlDbType.VarChar, 50).Value = Ceco.Codigo_Ceco;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                Ceco.Id_Ceco = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Centro de costo!! " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return Ceco;
        }
        /// <summary>
        /// Metodo que se usa para asegura que no se repita el Centro de costo
        /// </summary>
        /// <param name="Ceco">Nombre a buscar</param>
        /// <returns></returns>
        public async Task<string> Existe(string Ceco)
        {
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Centro_de_costo WHERE Codigo_Ceco = @Codigo_Ceco";
                    command.Parameters.AddWithValue("@Codigo_Ceco", Ceco);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        return "El codigo CeCo ya existe";
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
        /// Metodo para "eliminar" un CeCo y evitar que se use para otros procesos
        /// </summary>
        /// <param name="CeCo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CentroCosto> EliminarCeCo(int CeCo)
        {
            CentroCosto ccmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Centro_de_costo SET " +
                    "Activado = @Activado " +
                    "WHERE Id_Ceco = @Id_Ceco";
                Comm.CommandType = CommandType.Text;

                //Usar cuando se corrija el ingresar datos, porque por ahora no se como meter una clase
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = false;
                Comm.Parameters.Add("@Id_Ceco", SqlDbType.Int).Value = CeCo;

                //Comm.Parameters.Add("@Bien_Servicio", SqlDbType.VarChar, 50).Value = "Pan";
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    ccmod = await GetCeCo(Convert.ToInt32(reader["Id_Ceco"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el Centro de costo " + ex.Message);
            }
            finally
            {
                reader?.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return ccmod;
        }
        /// <summary>
        /// Obtienes un CeCo por su codigo unico
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CentroCosto> GetCeCo(string code)
        {
            //Parametro para guardar el objeto a mostrar
            CentroCosto cc = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Centro_de_costo where Codigo_Ceco = @Id_Ceco";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Ceco", SqlDbType.VarChar).Value = code;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    cc.Id_Ceco = Convert.ToInt32(reader["Id_CeCo"]);
                    cc.Nombre = (Convert.ToString(reader["NombreCeCo"])).Trim();
                    cc.Codigo_Ceco = (Convert.ToString(reader["Codigo_Ceco"])).Trim();
                    cc.Activado = Convert.ToBoolean(reader["Activado"]);

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Centro de costo " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return cc;
        }
    }
}

