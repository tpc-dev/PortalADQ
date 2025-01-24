using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioLiberadores : IRepositorioLiberadores
    {

        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioLiberadores(AccesoDatos CD)
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
        /// Creacion de un
        /// </summary>
        /// <param name="DP"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Liberadores> Nuevo(Liberadores L)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Liberadores " +
                                    "(Id_Usuario,Id_Departamento) " +
                                    "VALUES (@Id_Usuario,@Id_Departamento) " +
                                    "SELECT SCOPE_IDENTITY() AS Id_Liberador";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = L.Id_Usuario;
                Comm.Parameters.Add("@Id_Departamento", SqlDbType.Int).Value = L.Id_Departamento;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                L.Id_Liberador = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla de Departamento Usuario " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return L;
        }

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Id del objeto Liberadores a buscar</param>
        /// <returns>Retorna el objeto Relacion cuya Id se pide</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Liberadores> Get(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Liberadores L = new();
            Console.WriteLine(id);
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
                Comm.CommandText = "SELECT L.* , u.Nombre_Usuario, u.Correo_Usuario, d.Nombre  " +
                    "FROM dbo.Liberadores L " +
                    "INNER JOIN dbo.Usuario u ON u.Id_Usuario = L.Id_Usuario " +
                    "INNER JOIN dbo.Departamento d ON L.Id_Departamento = d.Id_Departamento " +
                    "where L.Id_Liberador = @id ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@id", SqlDbType.Int).Value = id;

                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    L.Id_Liberador = Convert.ToInt32(reader["Id_Liberador"]);
                    L.Nombre_Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    L.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);
                    L.Nombre_Departamento = Convert.ToString(reader["Nombre"]).Trim();
                    L.Id_Departamento = Convert.ToInt32(reader["Id_Departamento"]);
                    L.Correo = Convert.ToString(reader["Correo_Usuario"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Departamento Usuario " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return L;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos Relacion de la lsita</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Liberadores>> GetAll()
        {
            List<Liberadores> lista = new List<Liberadores>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = @"SELECT L.* , u.Nombre_Usuario , d.Nombre, u.Correo_Usuario 
                FROM dbo.Liberadores L 
                INNER JOIN dbo.Usuario u ON u.Id_Usuario = L.Id_Usuario 
                INNER JOIN dbo.Departamento d ON L.Id_Departamento = d.Id_Departamento  ";  // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Liberadores L = new();

                    L.Id_Liberador = Convert.ToInt32(reader["Id_Liberador"]);
                    L.Nombre_Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    L.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);
                    L.Nombre_Departamento = Convert.ToString(reader["Nombre"]).Trim();
                    L.Id_Departamento = Convert.ToInt32(reader["Id_Departamento"]);
                    L.Correo = Convert.ToString(reader["Correo_Usuario"]);


                    lista.Add(L);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Relacion " + ex.Message);
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
        /// <param name="DP">Objeto del tipo DepartamentoUsuario que se usará para modificar su homonimo por Id</param>
        /// <returns>Retorna el objeto Modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Liberadores> Modificar(Liberadores L)
        {

            Liberadores Lmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Liberadores SET " +
                    "Id_Usuario = @Id_Usuario," +
                    "Id_Departamento = @Id_Departamento " +
                    "WHERE Id_Liberador = @Id_Liberador";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Liberador", SqlDbType.Int).Value = L.Id_Liberador;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = L.Id_Usuario;
                Comm.Parameters.Add("@Id_Departamento", SqlDbType.Int).Value = L.Id_Departamento;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Lmod = await Get(Convert.ToInt32(reader["Id_Liberador"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el liberador " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Lmod;
        }
        /// <summary>
        /// Metodo que confirma si existe un objeto duplicado, un objeto duplicado es aquel que tiene sus datos unicos 
        /// o datos que lo diferencian repetidos en la base de datos
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="dep"></param>
        /// <returns></returns>
        public async Task<string> Existe(int Usuario, int  dep)
        {
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
   
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT Top 1 1 FROM dbo.Liberadores WHERE Id_Usuario = @Id_Usuario and Id_Departamento = @Id_Departamento";
                    command.Parameters.AddWithValue("@Id_Usuario", Usuario);
                    command.Parameters.AddWithValue("@Id_Departamento", dep);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        return "El Liberador ya existe";
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
        /// Metodo par obtener un liberador
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Liberadores> GetDep(int dep)
        {
            //Parametro para guardar el objeto a mostrar
            Liberadores L = new();
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
                Comm.CommandText = "SELECT L.Id_Liberador, u.Nombre_Usuario,u.Id_Usuario , d.Nombre,d.Id_Departamento " +
                    "FROM dbo.Liberadores L " +
                    "INNER JOIN dbo.Usuario u ON u.Id_Usuario = L.Id_Usuario " +
                    "INNER JOIN dbo.Departamento d ON L.Id_Departamento = d.Id_Departamento " +
                    "where L.Id_Departamento = @dep";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@dep", SqlDbType.Int).Value = dep;

                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {

                    L.Id_Liberador = Convert.ToInt32(reader["Id_Liberador"]);
                    L.Nombre_Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    L.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);
                    L.Nombre_Departamento = Convert.ToString(reader["Nombre"]).Trim();
                    L.Id_Departamento = Convert.ToInt32(reader["Id_Departamento"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Departamento Usuario " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return L;
        }
    }
}
