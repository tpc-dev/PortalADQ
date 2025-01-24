using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioReemplazos : IRepositorioReemplazos
    {
       
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioReemplazos(AccesoDatos CD)
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
        /// <param name="R">Objeto del tipo Reemplazos que se agregará a la base de datos</param>
        /// <returns>Retorna un objeto del tipo Reemplazos que se agrega a la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Reemplazos> NuevoReemplazos(Reemplazos R)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Reemplazos " +
                    "(Id_Usuario_Vacaciones,Id_Usuario_Reemplazante,Comentario,Fecha_Retorno,Valido) " +
                    "VALUES (@Id_Usuario_Vacaciones,@Id_Usuario_Reemplazante,@Comentario,@Fecha_Retorno,@Valido); " +
                    "SELECT SCOPE_IDENTITY() AS ID_Reemplazos";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Usuario_Vacaciones", SqlDbType.Int).Value = R.Id_Usuario_Vacaciones;
                Comm.Parameters.Add("@Id_Usuario_Reemplazante", SqlDbType.Int).Value = R.Id_Usuario_Reemplazante;
                Comm.Parameters.Add("@Comentario", SqlDbType.VarChar).Value = R.Comentario;
                Comm.Parameters.Add("@Fecha_Retorno", SqlDbType.DateTime).Value = R.Fecha_Retorno;
                Comm.Parameters.Add("@Valido", SqlDbType.Bit).Value = true;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                R.ID_Reemplazos = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Reemplazos " + ex.Message);
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
        /// <param name="id">Id del objeto Reemplazos que se quiere buscar</param>
        /// <returns>Retorna el objeto Reemplazos con la Id requerida</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Reemplazos> GetReemplazo(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Reemplazos Rem = new();
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
                Comm.CommandText = "SELECT R.*, U.Nombre_Usuario AS U_Vacaciones, U1.Nombre_Usuario AS U_Reemplazo  " +
                    "FROM dbo.Reemplazos R " +
                    "INNER JOIN Usuario U on U.Id_Usuario = R.Id_Usuario_Vacaciones " +
                    "INNER JOIN Usuario U1 on U1.Id_Usuario = R.Id_Usuario_Reemplazante " +
                    "where R.ID_Reemplazos = @ID";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                
                    Rem.ID_Reemplazos = Convert.ToInt32(reader["ID_Reemplazos"]);
                    Rem.Id_Usuario_Vacaciones = Convert.ToString(reader["U_Vacaciones"]).Trim();
                    Rem.Id_Usuario_Reemplazante = Convert.ToString(reader["U_Reemplazo"]).Trim();
                    Rem.Comentario = Convert.ToString(reader["Comentario"]).Trim();
                    Rem.Fecha_Retorno = (DateTime)reader["Fecha_Retorno"];
                    Rem.Valido = Convert.ToBoolean(reader["Valido"]);
                    Rem.N_IdV = Convert.ToInt32(reader["Id_Usuario_Vacaciones"]);
                    Rem.N_IdR = Convert.ToInt32(reader["Id_Usuario_Reemplazante"]);

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Reemplazos " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return Rem;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos del tipo Reemplazos de la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Reemplazos>> GetAllRemplazos()
        {
            List<Reemplazos> lista = new List<Reemplazos>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT R.*, U.Nombre_Usuario AS U_Vacaciones, U1.Nombre_Usuario AS U_Reemplazo  " +
                    "FROM dbo.Reemplazos R " +
                    "INNER JOIN Usuario U on U.Id_Usuario = R.Id_Usuario_Vacaciones " +
                    "INNER JOIN Usuario U1 on U1.Id_Usuario = R.Id_Usuario_Reemplazante " +
                    "Where R.Valido = @A"; // leer base datos 
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@A", SqlDbType.Bit).Value = true;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Reemplazos R = new();

                    R.ID_Reemplazos = Convert.ToInt32(reader["ID_Reemplazos"]);
                    R.Id_Usuario_Vacaciones = Convert.ToString(reader["U_Vacaciones"]).Trim();
                    R.Id_Usuario_Reemplazante = Convert.ToString(reader["U_Reemplazo"]).Trim();
                    R.Comentario = Convert.ToString(reader["Comentario"]).Trim();
                    R.Fecha_Retorno = (DateTime)reader["Fecha_Retorno"];
                    R.Valido = Convert.ToBoolean(reader["Valido"]);
                    R.N_IdV = Convert.ToInt32(reader["Id_Usuario_Vacaciones"]);
                    R.N_IdR = Convert.ToInt32(reader["Id_Usuario_Reemplazante"]);


                    lista.Add(R);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Reemplazos " + ex.Message);
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
        /// <param name="R"></param>
        /// <returns>Retorna el objeto Reemplazos modificado Objetos Reemplazos </returns>
        /// <exception cref="Exception"></exception>
        public async Task<Reemplazos> ModificarReemplazos(Reemplazos R)
        {
            Reemplazos Rmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Reemplazos SET " +

                    "Id_Usuario_Reemplazante = @IDR, " +
                    "Comentario = @Comentario, " +
     
                    "Valido = @Valido " +
                    "WHERE ID_Reemplazos = @ID_Reemplazos ";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@ID_Reemplazos", SqlDbType.Int).Value = R.ID_Reemplazos;
                Comm.Parameters.Add("@IDR", SqlDbType.Int).Value = R.N_IdR;
                Comm.Parameters.Add("@Comentario", SqlDbType.VarChar).Value = R.Comentario;
                Comm.Parameters.Add("@Valido", SqlDbType.Bit).Value = R.Valido;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Rmod = await GetReemplazo(Convert.ToInt32(reader["ID_Reemplazos"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el reemplazo " + ex.Message);
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