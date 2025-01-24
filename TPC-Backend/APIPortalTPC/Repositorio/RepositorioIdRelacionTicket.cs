using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioIdRelacionTicket : IRepositorioIdRelacionTicket
    {

        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioIdRelacionTicket(AccesoDatos CD)
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
        /// <param name="R">Objeto Id_RelacionTicket que se va a añadir a la base de datos</param>
        /// <returns>Retorna el objeto Id_RelacionTicket que fue añadida</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Id_RelacionTicket> NuevaRelacion(Id_RelacionTicket R)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Id_RelacionTicket " +
                    "(Id_Archivo,Id_Ticket) " +
                    "VALUES (@Id_Archivo,@Id_Ticket); " +
                    "SELECT SCOPE_IDENTITY() AS Id_RelacionTicket";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = R.Id_Archivo;
                Comm.Parameters.Add("@Id_Ticket", SqlDbType.Int).Value = R.Id_Ticket;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                int id = (int)idDecimal;
                R.IdRelacionTicket = id;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla de relaciones " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return R;
        }

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Id del objeto Id_RelacionTicket a buscar</param>
        /// <returns>Retorna el objeto Id_RelacionTicket cuya Id se pide</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Id_RelacionTicket> GetRelacion(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Id_RelacionTicket R = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Id_RelacionTicket " +
                    "where Id_Ticket  = @Id_Ticket ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Ticket", SqlDbType.Int).Value = id;

                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    R.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);
                    R.Id_Ticket = reader["Id_Ticket"] is DBNull ? 0 : Convert.ToInt32(reader["Id_Ticket"]);
                    R.IdRelacionTicket = Convert.ToInt32(reader["Id_RelacionTicket"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Id_RelacionTicket " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return R;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos Id_RelacionTicket de la lsita</returns>
        /// <exception cref="Exception"></exception>
        
        public async Task<IEnumerable<Id_RelacionTicket>> GetAllRelacion()
        {
            List<Id_RelacionTicket> lista = new List<Id_RelacionTicket>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Id_RelacionTicket"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Id_RelacionTicket R = new();
                    R.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);
                    R.Id_Ticket = reader["Id_Ticket"] is DBNull ? 0 : Convert.ToInt32(reader["Id_Ticket"]);
                    R.IdRelacionTicket = Convert.ToInt32(reader["Id_RelacionTicket"]);
                    lista.Add(R);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Id_RelacionTicket " + ex.Message);
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
        /// <param name="R">Objeto del tipo Id_RelacionTicket que se usará para modificar su homonimo por Id</param>
        /// <returns>Retorna el objeto Modificado</returns>
        /// <exception cref="Exception"></exception>
        
        public async Task<Id_RelacionTicket> ModificarRelacion(Id_RelacionTicket R)
        {
            Id_RelacionTicket Rmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Id_RelacionTicket SET " +
                    "Id_Archivo = @Id_Archivo, " +
                    "Id_Ticket = @Id_Ticket " +
                    "WHERE ID_Relacion = @ID_Relacion";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@IdRelacionTicket", SqlDbType.Int).Value = R.IdRelacionTicket;
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = R.Id_Archivo;

                Comm.Parameters.Add("@Id_Ticket", SqlDbType.Int).Value = R.Id_Ticket;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Rmod = await GetRelacion(Convert.ToInt32(reader["ID_Relacion"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la relacion " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Rmod;
        }

    }
}