using APIPortalTPC.Datos;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using System.Data;
using System.Data.SqlClient;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioCorreo : IRepositorioCorreo
    {
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioCorreo(AccesoDatos CD)
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
 
        public async Task<Correo> NuevoCorreo(Correo C)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Correo " +
                    "(Id_Ticket,FechaCreacion,CorreosEnviados,PrimerCorreo,UltimoCorreo,detalle,Numero_OC,Activado) " +
                    "VALUES (@Id_Ticket,@FechaCreacion,@CorreosEnviados,@PrimerCorreo,@UltimoCorreo,@detalle,@Numero_OC,@Activado) " +
                    "SELECT SCOPE_IDENTITY() AS Id_Correo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Ticket", SqlDbType.Int).Value = C.Id_Ticket;
                Comm.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = DateTime.Now;
                Comm.Parameters.Add("@CorreosEnviados", SqlDbType.Int).Value = 1;
                Comm.Parameters.Add("@PrimerCorreo", SqlDbType.DateTime).Value = DateTime.Now;
                Comm.Parameters.Add("@UltimoCorreo", SqlDbType.DateTime).Value = DateTime.Now;
                Comm.Parameters.Add("@detalle", SqlDbType.VarChar, 500).Value = C.detalle;
                Comm.Parameters.Add("@Numero_OC", SqlDbType.BigInt).Value = C.Numero_OC;
                Comm.Parameters.Add("Activado", SqlDbType.Bit).Value = true;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                C.Id_Correo = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Correos " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return C;
        }
        public async Task<Correo> GetCorreo(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Correo C = new ();
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
                Comm.CommandText = @"SELECT C.* ,U.Nombre_Usuario,T.Id_Usuario, P.Nombre_Fantasia, CeCo.NombreCeCo
                FROM dbo.Correo C 
                Inner join Ticket T on T.Id_Ticket = C.Id_Ticket 
                Inner join Usuario U on U.Id_Usuario = T.Id_Usuario 
                Inner join Proveedores P on P.Id_Proveedores = T.Id_Proveedor 
                Inner join Ordenes_estadisticas OE on T.Id_OE = OE.Id_Orden_Estadistica 
                Inner join Centro_de_costo CeCo on OE.Id_Centro_de_Costo = CeCo.Id_Ceco 
                where C.Id_Correo = @Id_Correo ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Correo", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                
                    C.Id_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                    C.Solicitante = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    C.ID_Solicitante = Convert.ToInt32(reader["Id_Usuario"]);
                    C.Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    C.CeCo = Convert.ToString(reader["NombreCeCo"]).Trim();
                    C.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                    C.CorreosEnviados = Convert.ToInt32(reader["CorreosEnviados"]);
                    C.PrimerCorreo = Convert.ToDateTime(reader["PrimerCorreo"]);
                    C.UltimoCorreo = Convert.ToDateTime(reader["UltimoCorreo"]);
                    C.detalle = Convert.ToString(reader["detalle"]).Trim();
                    C.Id_Correo = Convert.ToInt32(reader["Id_Correo"]);
                    C.Numero_OC = Convert.ToInt64(reader["Numero_OC"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Correo " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return C;
        }

        public async Task<IEnumerable<Correo>> GetAllCorreo()
        {
            List<Correo> lista = new List<Correo>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = @"SELECT C.* ,U.Nombre_Usuario,T.Id_Usuario, P.Nombre_Fantasia, CeCo.NombreCeCo
                FROM dbo.Correo C
                Inner join Ticket T on T.Id_Ticket = C.Id_Ticket
                Inner join Usuario U on U.Id_Usuario = T.Id_Usuario
                Inner join Proveedores P on P.Id_Proveedores = T.Id_Proveedor
                Inner join Ordenes_estadisticas OE on T.Id_OE = OE.Id_Orden_Estadistica
                Inner join Centro_de_costo CeCo on OE.Id_Centro_de_Costo = CeCo.Id_Ceco  
                where C.Activado = @A"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@A", SqlDbType.Int).Value = true;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Correo C = new();
                    C.Id_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                    C.Solicitante = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    C.ID_Solicitante = Convert.ToInt32(reader["Id_Usuario"]);
                    C.Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    C.CeCo = Convert.ToString(reader["NombreCeCo"]).Trim();
                    C.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                    C.CorreosEnviados = Convert.ToInt32(reader["CorreosEnviados"]);
                    C.PrimerCorreo = Convert.ToDateTime(reader["PrimerCorreo"]);
                    C.UltimoCorreo = Convert.ToDateTime(reader["UltimoCorreo"]);
                    C.detalle = Convert.ToString(reader["detalle"]).Trim();
                    C.Id_Correo = Convert.ToInt32(reader["Id_Correo"]);
                    C.Numero_OC = Convert.ToInt64(reader["Numero_OC"]);
                    C.Activado = Convert.ToBoolean(reader["Activado"]);
                    lista.Add(C);
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
        public async Task<Correo> ModificarCorreo(Correo C)
        {
            Correo Cmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Correo SET " +
                    "CorreosEnviados = @CorreosEnviados," +
                    "UltimoCorreo = @UltimoCorreo," +
                    "detalle = @detalle, " +
                    "Numero_OC = @Numero_OC, " +
                    "Activado = @A " +
                    "WHERE Id_Correo = @Id_Correo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Correo", SqlDbType.Int).Value = C.Id_Correo;
                Comm.Parameters.Add("CorreosEnviados",SqlDbType.Int).Value = C.CorreosEnviados;
                Comm.Parameters.Add("@detalle", SqlDbType.VarChar).Value = C.detalle;
                Comm.Parameters.Add("@Numero_OC", SqlDbType.BigInt).Value = C.Numero_OC;
                Comm.Parameters.Add("@UltimoCorreo", SqlDbType.DateTime).Value = C.UltimoCorreo;

                Comm.Parameters.Add("@A", SqlDbType.Bit).Value = C.Activado;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Cmod = await GetCorreo(Convert.ToInt32(reader["Id_Correo"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el correo " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm?.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Cmod;
        }
        /// <summary>
        /// Se obtiene el Correo por el id Ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Correo> GetCorreoPorTicket(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Correo C = new();
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
                Comm.CommandText = @"SELECT C.* ,U.Nombre_Usuario,T.Id_Usuario, P.Nombre_Fantasia, CeCo.NombreCeCo
                FROM dbo.Correo C 
                Inner join Ticket T on T.Id_Ticket = C.Id_Ticket 
                Inner join Usuario U on U.Id_Usuario = T.Id_Usuario 
                Inner join Proveedores P on P.Id_Proveedores = T.Id_Proveedor 
                Inner join Ordenes_estadisticas OE on T.Id_OE = OE.Id_Orden_Estadistica 
                Inner join Centro_de_costo CeCo on OE.Id_Centro_de_Costo = CeCo.Id_Ceco 
                where C.Id_Ticket = @T ";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@T", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido

                    C.Id_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                    C.Solicitante = Convert.ToString(reader["Nombre_Usuario"]).Trim();
                    C.ID_Solicitante = Convert.ToInt32(reader["Id_Usuario"]);
                    C.Proveedor = Convert.ToString(reader["Nombre_Fantasia"]).Trim();
                    C.CeCo = Convert.ToString(reader["NombreCeCo"]).Trim();
                    C.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                    C.CorreosEnviados = Convert.ToInt32(reader["CorreosEnviados"]);
                    C.PrimerCorreo = Convert.ToDateTime(reader["PrimerCorreo"]);
                    C.UltimoCorreo = Convert.ToDateTime(reader["UltimoCorreo"]);
                    C.detalle = Convert.ToString(reader["detalle"]).Trim();
                    C.Id_Correo = Convert.ToInt32(reader["Id_Correo"]);
                    C.Numero_OC = Convert.ToInt64(reader["Numero_OC"]);
                   
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Correo " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return C;
        }

        public async Task<string> Existe(int id)
        {
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Correo WHERE Id_Ticket = @T";
                    command.Parameters.AddWithValue("@T", id);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        return "El Correo ya existe";
                    }
                    else
                    {
                        reader.Close();
                        return "ok";
                    }
                }
            }
        }
    }
}
