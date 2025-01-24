using BaseDatosTPC;
using APIPortalTPC.Datos;
using System.Data.SqlClient;
using System.Data;
using NPOI.SS.Formula.Functions;
namespace APIPortalTPC.Repositorio
{
    public class RepositorioArchivo : IRepositorioArchivo
    {
        /// <value>Variable que guarda el string para la conexion con la base de datos </value>
        private readonly string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioArchivo(AccesoDatos CD)
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
        ///  Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Corresponde al Id a buscar</param>
        /// <returns>El archivo con la id a buscar</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Archivo> GetArchivo(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Archivo a = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Archivo " +
                    "where Id_Archivo = @Id_Archivo";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = id;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    a.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);

                    a.NombreDoc = Convert.ToString(reader["NombreDoc"]);
                try
                       {
                           a.ArchivoDoc = (byte[])(reader["ArchivoDoc"]);
                       }
                       catch (InvalidCastException)
                       {

                           a.ArchivoDoc = [0];
                       }
  
                } 


            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Archivo " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader?.Close();
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return a;
        }
        /// <summary>
        /// Metodo que retorna una lista con los Archivos
        /// </summary>
        /// <returns>Una lista con los archivos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Archivo>> GetAllArchivo()
        {
            List<Archivo> lista = new List<Archivo>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Archivo"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Archivo a = new ();
                    a.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);

                    a.NombreDoc = Convert.ToString(reader["NombreDoc"]);
                    lista.Add(a);
                }
            }   
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Archivo " + ex.Message);
            }
            finally
            {
                reader?.Close();
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return lista;
        }
        /// <summary>
        /// Pide un objetivo ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="A">Corresponde al Objeto Archivo a reemplazar</param>
        /// <returns>Retorna el objeto Archivo modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Archivo> ModificarArchivo(Archivo A)
        {
            Archivo? Archmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Archivo SET " +
                    "NombreDoc = @NombreDoc, " +
                    "ArchivoDoc = @ArchivoDoc " +
                    "WHERE Id_Archivo = @Id_Archivo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = A.Id_Archivo;
                Comm.Parameters.Add("@NombreDoc", SqlDbType.VarChar,50).Value = A.NombreDoc;
                Comm.Parameters.Add("@ArchivoDoc", SqlDbType.VarBinary).Value = A.ArchivoDoc;

                // Crear un objeto de la clase Archivo


                Comm.Parameters.Add("@ArchivoDoc", SqlDbType.VarBinary, -1).Value = A.ArchivoDoc;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Archmod = await GetArchivo(Convert.ToInt32(reader["Id_Archivo"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el Archivo " + ex.Message);
            }
            finally
            {
                reader?.Close();

                Comm?.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Archmod;
        }
        /// <summary>
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="A">Corresponde al Objeto Archivo a añadir</param>
        /// <returns>Retorna el Objeto Archivo que se ha añadido</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Archivo> NuevoArchivo(Archivo A)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Archivo " +
                    "(ArchivoDoc,NombreDoc) " +
                    "VALUES (@ArchivoDoc,@NombreDoc); " +
                    "SELECT SCOPE_IDENTITY() AS Id_Archivo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = A.Id_Archivo;
                Comm.Parameters.Add("@NombreDoc", SqlDbType.VarChar, 50).Value = A.NombreDoc;
                Comm.Parameters.Add("@ArchivoDoc",SqlDbType.VarBinary).Value = A.ArchivoDoc;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
        
               A.Id_Archivo = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Archivo " + ex.Message);
            }
            finally
            {
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return A;
        }
        
        /// <summary>
        /// Metodo que descarga el archivo asociado
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public async Task<Archivo> DescargarArchivo(int id)
        { //Parametro para guardar el objeto a mostrar
            Archivo a = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Archivo " +
                    "where Id_Archivo = @Id_Archivo";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = id;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    a.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);
                    a.NombreDoc = Convert.ToString(reader["NombreDoc"]);
                    try
                    {
                        a.ArchivoDoc = (byte[])(reader["ArchivoDoc"]);
                    }
                    catch (InvalidCastException)
                    {

                      return null;
                    }
                  
                }


            }
            catch (SqlException ex)
            {
                return null;
            }
            finally
            {
                //Se cierran los objetos 
                reader?.Close();
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            
            }

            return a;
        }

    }

}

