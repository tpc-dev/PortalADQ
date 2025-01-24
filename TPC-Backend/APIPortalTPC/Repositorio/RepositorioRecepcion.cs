using APIPortalTPC.Datos;
using BaseDatosTPC;
using NPOI.SS.Formula.Functions;
using System.Data;
using System.Data.SqlClient;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioRecepcion : IRepositorioRecepcion
    {
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioRecepcion(AccesoDatos CD)
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

        public async Task<Recepcion> NuevaRecepcion(Recepcion R)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Recepcion " +
                    "(Id_Correo,FechaEnvio,FechaRespuesta,Comentarios,Respuesta) " +
                    "VALUES (@Id_Correo,@FechaEnvio,@FechaRespuesta,@Comentarios,@Respuesta) " +
                    "SELECT SCOPE_IDENTITY() AS Id_Correo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Correo", SqlDbType.Int).Value = R.Id_Correo;
                Comm.Parameters.Add("@FechaEnvio", SqlDbType.DateTime).Value = DateTime.Now;
                if (R.FechaRespuesta.HasValue)
                    Comm.Parameters.Add("@FechaRespuesta", SqlDbType.DateTime).Value = R.FechaRespuesta;
                else
                    Comm.Parameters.Add("@FechaRespuesta", SqlDbType.DateTime).Value = DBNull.Value;

                Comm.Parameters.Add("@Respuesta", SqlDbType.VarChar, 500).Value = R.Respuesta;
                Comm.Parameters.Add("@Comentarios", SqlDbType.VarChar, 500).Value = R.Comentarios;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                R.Id_Recepcion = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Recepcion " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return R;
        }
        public async Task<Recepcion> GetRecepcion(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Recepcion R = new();
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
                Comm.CommandText = @"SELECT  R.*, T.ID_Ticket, T.Id_OE, U.Nombre_Usuario, P.Nombre_Fantasia, CeCo.NombreCeCo, OC-Numero_OC
                FROM dbo.Recepcion R
                inner join dbo.Correo C on R.Id_Correo = C.Id_Correo  
                inner join dbo.Ticket T on C.Id_Ticket = T.ID_Ticket 
                inner join dbo.Usuario U on U.Id_Usuario = T.Id_Usuario 
                inner join dbo.Proveedores P on P.ID_Proveedores = T.ID_Proveedor
                inner join dbo.Ordenes_estadisticas OE on T.Id_OE = OE.Id_Orden_Estadistica
                inner join dbo.Centro_de_costo CeCo on OE.Id_Centro_de_Costo = CeCo.Id_Ceco
                inner join dbo.Orden_de_Compra OC on OC.Id_Ticket = T.ID_Ticket
                where R.Id_Recepcion = @Id_Recepcion ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Recepcion", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                    R.Id_Recepcion = Convert.ToInt32(reader["Id_Recepcion"]);
                    R.Id_Correo = Convert.ToInt32(reader["Id_Correo"]);
                    R.FechaEnvio = Convert.ToDateTime(reader["FechaEnvio"]);
                    R.FechaRespuesta = reader["FechaRespuesta"] is DBNull ? (DateTime?)null : (DateTime)reader["FechaRespuesta"];
                    R.Respuesta = Convert.ToString(reader["Respuesta"]).Trim();
                    R.Comentarios = Convert.ToString(reader["Comentarios"]).Trim();
                    R.Ceco = Convert.ToString(reader["NombreCeCo"]).Trim();
                    R.Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    R.Usuario= Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    R.N_OC = Convert.ToInt64(reader["Numero_OC"]);
                    R.N_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Recepcion " + ex.Message);
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
        public async Task<IEnumerable<Recepcion>> GetAllRecepcion()
        {
            List<Recepcion> lista = new List<Recepcion>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = @"SELECT  R.*, T.ID_Ticket, T.Id_OE, U.Nombre_Usuario, P.Nombre_Fantasia, CeCo.NombreCeCo, OC.Numero_OC
                FROM dbo.Recepcion R
                inner join dbo.Correo C on R.Id_Correo = C.Id_Correo  
                inner join dbo.Ticket T on C.Id_Ticket = T.ID_Ticket 
                inner join dbo.Usuario U on U.Id_Usuario = T.Id_Usuario 
                inner join dbo.Proveedores P on P.ID_Proveedores = T.ID_Proveedor
                inner join dbo.Ordenes_estadisticas OE on T.Id_OE = OE.Id_Orden_Estadistica
                inner join dbo.Centro_de_costo CeCo on OE.Id_Centro_de_Costo = CeCo.Id_Ceco
                inner join dbo.Orden_de_Compra OC on OC.Id_Ticket = T.ID_Ticket ";
                // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Recepcion R = new();
                    R.Id_Recepcion = Convert.ToInt32(reader["Id_Recepcion"]);
                    R.Id_Correo = Convert.ToInt32(reader["Id_Correo"]);
                    R.FechaEnvio = Convert.ToDateTime(reader["FechaEnvio"]);
                    R.FechaRespuesta = reader["FechaRespuesta"] is DBNull ? (DateTime?)null : (DateTime)reader["FechaRespuesta"];
                    R.Respuesta = Convert.ToString(reader["Respuesta"]).Trim();
                    R.Comentarios = Convert.ToString(reader["Comentarios"]).Trim();
                    R.Ceco = Convert.ToString(reader["NombreCeCo"]).Trim();
                    R.Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    R.Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    R.N_OC = Convert.ToInt64(reader["Numero_OC"]);
                    R.N_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                    lista.Add(R);
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
        public async Task<Recepcion> ModificarRecepcion(Recepcion R)
        {
            Recepcion Rmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Recepcion SET " +
                    "FechaRespuesta = @FechaRespuesta, " +
                    "Respuesta = @Respuesta " +
                    "WHERE Id_Recepcion = @Id_Recepcion";

                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Recepcion", SqlDbType.Int).Value = R.Id_Recepcion;
                if(R.FechaRespuesta.HasValue)
                    Comm.Parameters.Add("@FechaRespuesta", SqlDbType.DateTime).Value = R.FechaRespuesta;
                else
                    Comm.Parameters.Add("@FechaRespuesta", SqlDbType.DateTime).Value = DBNull.Value;

                Comm.Parameters.Add("@Respuesta", SqlDbType.VarChar,500).Value = R.Respuesta;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Rmod = await GetRecepcion(Convert.ToInt32(reader["Id_Recepcion"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la recepcion " + ex.Message);
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
        public async Task<Recepcion> GetRecepcionPorCorreo(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Recepcion R = new();
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
                Comm.CommandText = @"SELECT  R.*, T.ID_Ticket, T.Id_OE, U.Nombre_Usuario, P.Nombre_Fantasia, CeCo.NombreCeCo, OC-Numero_OC
                FROM dbo.Recepcion R
                inner join dbo.Correo C on R.Id_Correo = C.Id_Correo  
                inner join dbo.Ticket T on C.Id_Ticket = T.ID_Ticket 
                inner join dbo.Usuario U on U.Id_Usuario = T.Id_Usuario 
                inner join dbo.Proveedores P on P.ID_Proveedores = T.ID_Proveedor
                inner join dbo.Ordenes_estadisticas OE on T.Id_OE = OE.Id_Orden_Estadistica
                inner join dbo.Centro_de_costo CeCo on OE.Id_Centro_de_Costo = CeCo.Id_Ceco
                inner join dbo.Orden_de_Compra OC on OC.Id_Ticket = T.ID_Ticket
                where R.Id_Correo = @Id_Correo ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Correo", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                    R.Id_Recepcion = Convert.ToInt32(reader["Id_Recepcion"]);
                    R.Id_Correo = Convert.ToInt32(reader["Id_Correo"]);
                    R.FechaEnvio = Convert.ToDateTime(reader["FechaEnvio"]);
                    R.FechaRespuesta = reader["FechaRespuesta"] is DBNull ? (DateTime?)null : (DateTime)reader["FechaRespuesta"];
                    R.Respuesta = Convert.ToString(reader["Respuesta"]).Trim();
                    R.Comentarios = Convert.ToString(reader["Comentarios"]).Trim();
                    R.Ceco = Convert.ToString(reader["NombreCeCo"]).Trim();
                    R.Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    R.Usuario = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    R.N_OC = Convert.ToInt64(reader["Numero_OC"]);
                    R.N_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Recepcion " + ex.Message);
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
        public async Task<string> Existe(int id_correo)
        {

            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Recepcion WHERE Id_Recepcion = @R";
                    command.Parameters.AddWithValue("@R", id_correo);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        return "Existe Correo!";
                    }
                    reader.Close();

                 
                        reader.Close();
                        return "ok";
                  
                }
            }
        }
    }
}
