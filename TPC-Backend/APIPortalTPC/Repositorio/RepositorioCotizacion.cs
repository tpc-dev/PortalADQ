using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioCotizacion : IRepositorioCotizacion
    {

        private readonly string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioCotizacion(AccesoDatos CD)
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
        /// <param name="id">Id del objeto Cotizacion a buscar</param>
        /// <returns>Regresa el objeto Cotizacion a buscar por id</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Cotizacion> GetCotizacion(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Cotizacion cot = new();
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
                Comm.CommandText = @"SELECT c.*, u.Nombre_Usuario, b.Bien_Servicio
                FROM dbo.Cotizacion c 
                INNER JOIN dbo.Usuario u ON c.Id_Solicitante = u.Id_Usuario 
                LEFT JOIN dbo.Bien_Servicio b ON c.ID_Bien_Servicio = b.ID_Bien_Servicio 
                where C.ID_Cotizacion = @ID_Cotizacion";

                Comm.Parameters.Add("@ID_Cotizacion", SqlDbType.Int).Value = id;
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                if (reader.Read())
                {

                    cot = new()
                    {
                        //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                        ID_Cotizacion = Convert.ToInt32(reader["ID_Cotizacion"]),
                        Id_Solicitante = Convert.ToString(reader["Nombre_Usuario"]).Trim(),
                        IdS = Convert.ToInt32(reader["Id_Solicitante"]),
                    };

                    object fechaCreacionCotizacionObject = reader["Fecha_Creacion_Cotizacion"];
                    cot.Fecha_Creacion_Cotizacion = (DateTime)fechaCreacionCotizacionObject;
                    cot.Estado = (Convert.ToString(reader["Estado"])).Trim();
                    cot.Detalle = Convert.ToString(reader["Detalle"]).Trim();
                    cot.Id_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    cot.Bien_Servicio = Convert.ToString(reader["Bien_Servicio"]).Trim();
                    if (!reader.IsDBNull(reader.GetOrdinal("Solped")))
                    {
                        cot.Solped = Convert.ToInt64(reader["Solped"]);
                    }
                    else
                    {
                        //Se asigna 0
                        cot.Solped = 0;
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Cotizaciones " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader?.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return cot;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Lista con todos los objetos Cotizacion de la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Cotizacion>> GetAllCotizacion()
        {
            List<Cotizacion> lista = new List<Cotizacion>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = @"SELECT c.*, u.Nombre_Usuario, b.Bien_Servicio, b.ID_Bien_Servicio  , u.Id_Usuario 
                FROM dbo.Cotizacion c 
                INNER JOIN dbo.Usuario u ON c.Id_Solicitante = u.Id_Usuario 
                LEFT JOIN dbo.Bien_Servicio b ON c.ID_Bien_Servicio = b.ID_Bien_Servicio ";// leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Cotizacion cot = new()
                    {
                        //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                        ID_Cotizacion = Convert.ToInt32(reader["ID_Cotizacion"]),
                        Id_Solicitante = Convert.ToString(reader["Nombre_Usuario"]).Trim(),
                        IdS = Convert.ToInt32(reader["Id_Solicitante"]),
                    };

                    object fechaCreacionCotizacionObject = reader["Fecha_Creacion_Cotizacion"];

                    cot.Fecha_Creacion_Cotizacion = (DateTime)fechaCreacionCotizacionObject;
                    cot.Estado = (Convert.ToString(reader["Estado"])).Trim();
                    cot.Detalle = Convert.ToString(reader["Detalle"]).Trim();
                    cot.Bien_Servicio = Convert.ToString(reader["Bien_Servicio"]).Trim();
                    cot.Bien_Servicio = Convert.ToString(reader["Bien_Servicio"]).Trim();
                    cot.Id_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("Solped")))
                    {
                        cot.Solped = Convert.ToInt64(reader["Solped"]);
                    }
                    else
                    {
                        //Se asigna 0
                        cot.Solped = 0;
                    }

                    lista.Add(cot);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Cotización " + lista + ex.Message);
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
        /// <param name="cotizacion">Objeto del tipo Cotizacion que se usará para reemplazarlo en la base de datos</param>
        /// <returns>Retorna el objeto que va a cambiar la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Cotizacion> ModificarCotizacion(Cotizacion cotizacion)
        {
            Cotizacion cotmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Cotizacion SET " +
                                   "Estado = @Estado, " +
                                   "ID_Bien_Servicio = @ID_Bien_Servicio, " +
                                   "Detalle = @Detalle, " +
                                   "Solped = @Solped " +
                                   "WHERE ID_Cotizacion = @ID_Cotizacion";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = cotizacion.Estado;
                if (cotizacion.Id_Bien_Servicio != null)
                    Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = cotizacion.Id_Bien_Servicio;
                else Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = DBNull.Value;
                if (cotizacion.Detalle != null)
                    Comm.Parameters.Add("@Detalle", SqlDbType.VarChar, 500).Value = cotizacion.Detalle;
                else Comm.Parameters.Add("@Detalle", SqlDbType.VarChar, 500).Value = "";
                if (cotizacion.Solped != null)
                    Comm.Parameters.Add("@Solped", SqlDbType.BigInt).Value = cotizacion.Solped;
                else Comm.Parameters.Add("@Solped", SqlDbType.BigInt).Value = 0;
                Comm.Parameters.Add("@ID_Cotizacion", SqlDbType.Int).Value = cotizacion.ID_Cotizacion;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    cotmod = await GetCotizacion(Convert.ToInt32(reader["ID_Cotizacion"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la cotización " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return cotmod;
        }

        /// <summary>
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="cotizacion">Objeto del tipo Cotizacion que va a añadirse a la base de datos</param>
        /// <returns>Retorna el objeto Cotizacion que se va a añadir</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Cotizacion> NuevaCotizacion(Cotizacion cotizacion)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO " +
                    "Cotizacion (Id_Solicitante,Fecha_Creacion_Cotizacion,Estado,ID_Bien_Servicio,Detalle,Solped) " +
                    "VALUES (@Id_Solicitante,@Fecha_Creacion_Cotizacion,@Estado,@ID_Bien_Servicio,@Detalle,@Solped) " +
                    "SELECT SCOPE_IDENTITY() AS ID_Cotizacion";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Solicitante", SqlDbType.Int).Value = cotizacion.Id_Solicitante;
                Comm.Parameters.Add("@Fecha_Creacion_Cotizacion", SqlDbType.DateTime).Value = DateTime.Now;
                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = cotizacion.Estado;
                if (cotizacion.Id_Bien_Servicio != null)
                    Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = cotizacion.Id_Bien_Servicio;
                else
                    Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = 0;

                if (cotizacion.Detalle != null)
                    Comm.Parameters.Add("@Detalle", SqlDbType.VarChar, 500).Value = cotizacion.Detalle;
                else Comm.Parameters.Add("@Detalle", SqlDbType.VarChar, 500).Value = "Sin detalle";

                if (cotizacion.Solped != null)
                {
                    Comm.Parameters.Add("@Solped", SqlDbType.BigInt).Value = cotizacion.Solped;
                }
                else
                {
                    Comm.Parameters.Add("@Solped", SqlDbType.BigInt).Value = 0;
                }

                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                cotizacion.ID_Cotizacion = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Cotizacion " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return cotizacion;
        }

        /// <summary>
        /// Busca la cotizacion por ID para "Eliminar" la cotizacion
        /// </summary>
        /// <param name="cotizacion"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Cotizacion> EliminarCotizacion(int cotizacion)
        {
            Cotizacion cotmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Cotizacion SET " +
                                   "Estado = @Estado, " +
                                   "Activado = @Activado " +
                                   "WHERE ID_Cotizacion = @ID_Cotizacion";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = "Cancelado";
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = false;
                Comm.Parameters.Add("@ID_Cotizacion", SqlDbType.Int).Value = cotizacion;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    cotmod = await GetCotizacion(Convert.ToInt32(reader["ID_Cotizacion"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la cotización " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return cotmod;
        }

    }

}