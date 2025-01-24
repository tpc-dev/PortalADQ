using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioProveedores : IRepositorioProveedores
    {
       
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioProveedores(AccesoDatos CD)
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
        /// <param name="id">Id del proveedor a buscar</param>
        /// <returns>Retorna objeto del tipo Proveedor con la Id solicitada</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Proveedores> GetProveedor(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Proveedores P = new();
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
                Comm.CommandText = "SELECT P.*, BS.Bien_Servicio " +
                    "FROM dbo.Proveedores P " +
                    "INNER JOIN dbo.Bien_Servicio BS ON BS.ID_Bien_Servicio = P.ID_Bien_Servicio " +
                    "where P.ID_Proveedores = @ID_Proveedores";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID_Proveedores", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    P.Rut_Proveedor = (Convert.ToString(reader["Rut_Proveedor"])).Trim();
                    P.Razon_Social = (Convert.ToString(reader["Razon_social"])).Trim();
                    P.Nombre_Fantasia = (Convert.ToString(reader["Nombre_Fantasia"])).Trim();
                    P.ID_Bien_Servicio = Convert.ToString(reader["Bien_Servicio"]).Trim();
                    P.Direccion = (Convert.ToString(reader["Direccion"])).Trim();
                    P.Comuna = (Convert.ToString(reader["Comuna"])).Trim();
                    P.Correo_Proveedor = (Convert.ToString(reader["Correo_Proveedor"])).Trim();
                    P.Telefono_Proveedor = Convert.ToString(reader["Telefono_Proveedor"]).Trim();
                    P.Cargo_Representante = (Convert.ToString(reader["Cargo_Representante"])).Trim();
                    P.Nombre_Representante = (Convert.ToString(reader["Nombre_Representante"])).Trim();
                    P.Email_Representante = (Convert.ToString(reader["Email_Representante"])).Trim();
                    P.Estado = Convert.ToBoolean(reader["Estado"]);
                    P.N_Cuenta = Convert.ToString(reader["N_Cuenta"]);
                    P.Banco = (Convert.ToString(reader["Banco"])).Trim();
                    P.Swift = (Convert.ToString(reader["Swift"])).Trim();
                    P.ID_Proveedores = Convert.ToInt32(reader["ID_Proveedores"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Proveedores " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return P;
        }
        /// <summary>
        /// Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="P">Objeto del tipo Proveedor que se usara para modificar</param>
        /// <returns>Retorna el objeto Proveedores que se modifico</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Proveedores> ModificarProveedor(Proveedores P)
        {
            string rut = P.Rut_Proveedor.Replace(".", "").Replace("-", "").Replace(" ", "").ToUpper();
            if (!CalcularDigitoVerificador(rut))
            {
                throw new Exception("Error de Rut ");
            }
            else {

             rut = RutNormalizado(rut);
            Proveedores Pmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Proveedores SET " +
                    "Rut_Proveedor = @Rut_Proveedor," +
                    "Razon_social = @Razon_social, " +
                    "Nombre_Fantasia = @Nombre_Fantasia, " +
                    "ID_Bien_Servicio = @ID_Bien_Servicio, " +
                    "Direccion = @Direccion, " +
                    "Comuna = @Comuna, " +
                    "Correo_Proveedor = @Correo_Proveedor, " +
                    "Telefono_Proveedor = @Telefono_Proveedor, " +
                    "Cargo_Representante = @Cargo_Representante, " +
                    "Nombre_Representante = @Nombre_Representante, " +
                    "Email_Representante = @Email_Representante, " +
                    "Estado = @Estado, " +
                    "N_Cuenta = @N_Cuenta, " +
                    "Banco = @Banco, " +
                    "Swift = @Swift " +
                    "WHERE ID_Proveedores = @ID_Proveedores";
                Comm.CommandType = CommandType.Text;
                    Comm.Parameters.Add("@Rut_Proveedor", SqlDbType.VarChar, 500).Value = rut;
                    Comm.Parameters.Add("@Razon_social", SqlDbType.VarChar, 500).Value = P.Razon_Social;
                    Comm.Parameters.Add("@Nombre_Fantasia", SqlDbType.VarChar, 500).Value = P.Nombre_Fantasia;


                    Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.VarChar, 500).Value = P.ID_Bien_Servicio;
                    Comm.Parameters.Add("@Direccion", SqlDbType.VarChar, 500).Value = P.Direccion;
                    Comm.Parameters.Add("@Comuna", SqlDbType.VarChar, 500).Value = P.Comuna;

                    Comm.Parameters.Add("@Correo_Proveedor", SqlDbType.VarChar, 500).Value = P.Correo_Proveedor;
                    Comm.Parameters.Add("@Telefono_Proveedor", SqlDbType.VarChar, 500).Value = P.Telefono_Proveedor;
                    Comm.Parameters.Add("@Cargo_Representante", SqlDbType.VarChar, 500).Value = P.Cargo_Representante;

                    Comm.Parameters.Add("@Nombre_Representante", SqlDbType.VarChar, 500).Value = P.Nombre_Representante;
                    Comm.Parameters.Add("@Email_Representante", SqlDbType.VarChar, 500).Value = P.Email_Representante;
                    Comm.Parameters.Add("@Estado", SqlDbType.Bit).Value = P.Estado;

                    Comm.Parameters.Add("@N_Cuenta", SqlDbType.VarChar, 500).Value = P.N_Cuenta;
                    Comm.Parameters.Add("@Banco", SqlDbType.VarChar, 500).Value = P.Banco;
                    Comm.Parameters.Add("@Swift", SqlDbType.VarChar, 500).Value = "0";

                    Comm.Parameters.Add("@ID_Proveedores", SqlDbType.Int).Value = P.ID_Proveedores;


                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Pmod = await GetProveedor(Convert.ToInt32(reader["ID_Proveedores"]));
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
            return Pmod;
        }
    }
        /// <summary>
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="P">Objeto del tipo Proveedores que se agregará a la base de datos</param>
        /// <returns>Retorna el objeto Proveedores que se agrego a la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Proveedores> NuevoProveedor(Proveedores P)
        {
            string rut = P.Rut_Proveedor;
            rut = rut.Replace(".", "").Replace("-", "").Replace(" ", "").ToUpper();
    
            if (!CalcularDigitoVerificador(rut))
            {
                throw new Exception("Error de Rut ");
            
            }
            else
            {
                rut = RutNormalizado(rut);
                SqlConnection sql = conectar();
                SqlCommand? Comm = null;
                try
                {
                    sql.Open();
                    Comm = sql.CreateCommand();
                    Comm.CommandText = "INSERT INTO Proveedores " +
                                       "(Rut_Proveedor, Razon_Social, Nombre_Fantasia, ID_Bien_Servicio, Direccion, Comuna, Correo_Proveedor, Telefono_Proveedor, Nombre_Representante, Email_Representante, N_Cuenta, Banco, Swift, Estado, Cargo_Representante) " +
                                       "VALUES (@Rut_Proveedor, @Razon_Social, @Nombre_Fantasia, @ID_Bien_Servicio, @Direccion, @Comuna, @Correo_Proveedor, @Telefono_Proveedor, @Nombre_Representante, @Email_Representante, @N_Cuenta, @Banco, @Swift, @Estado,@Cargo_Representante) " +
                                       "SELECT SCOPE_IDENTITY() AS ID_Proveedores";
                    Comm.CommandType = CommandType.Text;
                    Comm.Parameters.Add("@Rut_Proveedor", SqlDbType.VarChar, 50).Value = rut;
                    Comm.Parameters.Add("@Razon_Social", SqlDbType.VarChar, 50).Value = P.Razon_Social;
                    Comm.Parameters.Add("@Nombre_Fantasia", SqlDbType.VarChar, 50).Value = P.Razon_Social;
                    Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = P.ID_Bien_Servicio;
                    Comm.Parameters.Add("@Direccion", SqlDbType.VarChar, 50).Value = P.Razon_Social;
                    Comm.Parameters.Add("@Comuna", SqlDbType.VarChar, 50).Value = P.Comuna;
                    Comm.Parameters.Add("@Correo_Proveedor", SqlDbType.VarChar, 50).Value = P.Correo_Proveedor;
                    Comm.Parameters.Add("@Telefono_Proveedor", SqlDbType.VarChar, 50).Value = P.Telefono_Proveedor;
                    Comm.Parameters.Add("@Nombre_Representante", SqlDbType.VarChar, 50).Value = P.Nombre_Representante;
                    Comm.Parameters.Add("@Email_Representante", SqlDbType.VarChar, 50).Value = P.Email_Representante;

                    if (string.IsNullOrEmpty(P.N_Cuenta))
                        Comm.Parameters.Add("@N_Cuenta", SqlDbType.VarChar, 50).Value = "";
                    else
                        Comm.Parameters.Add("@N_Cuenta", SqlDbType.VarChar, 50).Value = P.N_Cuenta;

                    if (string.IsNullOrEmpty(P.Banco))
                        Comm.Parameters.Add("@Banco", SqlDbType.VarChar, 50).Value = "";
                    else
                        Comm.Parameters.Add("@Banco", SqlDbType.VarChar, 50).Value = P.Banco;
                    if (string.IsNullOrEmpty(P.Swift))
                        Comm.Parameters.Add("@Swift", SqlDbType.VarChar, 50).Value = "";
                    else
                        Comm.Parameters.Add("@Swift", SqlDbType.VarChar, 50).Value = P.Swift;
                    if (string.IsNullOrEmpty(P.Cargo_Representante))
                        Comm.Parameters.Add("@Cargo_Representante", SqlDbType.VarChar, 50).Value = "";
                    else
                        Comm.Parameters.Add("@Cargo_Representante", SqlDbType.VarChar, 50).Value = P.Cargo_Representante;
                    Comm.Parameters.Add("@Estado", SqlDbType.Bit).Value = true;

                    decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                    int id = (int)idDecimal;
                    P.ID_Proveedores = id;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error creando los datos en tabla Proveedores " + ex.Message);
                }
                finally
                {
                    Comm.Dispose();
                    sql.Close();
                    sql.Dispose();
                }
                return P;
            }
        }

        /// <summary>
        /// Metodo que permite ver si existe el proveedor de un bien o servicio en especifico
        /// </summary>
        /// <param name="id_bs">Id del biservicio relacionado al proveedor</param>
        /// <param name="rut">Rut del proveedor</param>
        /// <returns></returns>
        public async Task<string> Existe(string rut,string bs)
        {
            rut = rut.Replace(".", "").Replace("-", "").Replace(" ", "").ToUpper();
            rut = RutNormalizado(rut);
            if (!CalcularDigitoVerificador(rut))
            {
                return "Rut no valido";
            }
            else { 
                using (SqlConnection sqlConnection = conectar())
                {
                    sqlConnection.Open();
                
                    using (SqlCommand command = new SqlCommand())
                    {
                        int valuebs = int.Parse(bs);
                        command.Connection = sqlConnection;
                        command.CommandText = "SELECT TOP 1 1 FROM dbo.Proveedores WHERE Rut_Proveedor = @rut and ID_Bien_Servicio = @bs";
                        command.Parameters.AddWithValue("@bs", valuebs); 
                        command.Parameters.AddWithValue("@rut", RutNormalizado(rut));
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            return "El rut proveedor con ese bien/servicio ya existe";
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

        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna lista con todos los proveedores de la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Proveedores>> GetAllProveedores()
        {
            List<Proveedores> lista = new List<Proveedores>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT P.*, BS.Bien_Servicio " +
                    "FROM dbo.Proveedores P " +
                    "INNER JOIN dbo.Bien_Servicio BS ON BS.ID_Bien_Servicio = P.ID_Bien_Servicio"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Proveedores P = new();
                    P.Rut_Proveedor = (Convert.ToString(reader["Rut_Proveedor"])).Trim();
                    P.Razon_Social = (Convert.ToString(reader["Razon_social"])).Trim();
                    P.Nombre_Fantasia = (Convert.ToString(reader["Nombre_Fantasia"])).Trim();
                    P.ID_Bien_Servicio = Convert.ToString(reader["Bien_Servicio"]).Trim();
                    P.Direccion = (Convert.ToString(reader["Direccion"])).Trim();
                    P.Comuna = (Convert.ToString(reader["Comuna"])).Trim();
                    P.Correo_Proveedor = (Convert.ToString(reader["Correo_Proveedor"])).Trim();
                    P.Telefono_Proveedor = Convert.ToString(reader["Telefono_Proveedor"]).Trim();
                    P.Cargo_Representante = (Convert.ToString(reader["Cargo_Representante"])).Trim();
                    P.Nombre_Representante = (Convert.ToString(reader["Nombre_Representante"])).Trim();
                    P.Email_Representante = (Convert.ToString(reader["Email_Representante"])).Trim();
                    P.Estado = Convert.ToBoolean(reader["Estado"]);
                    P.Cargo_Representante = Convert.ToString(reader["Cargo_Representante"]).Trim();
                    P.N_Cuenta = Convert.ToString(reader["N_Cuenta"]).Trim();
                    P.Banco = (Convert.ToString(reader["Banco"])).Trim();
                    P.Swift = (Convert.ToString(reader["Swift"])).Trim();
                    P.ID_Proveedores = Convert.ToInt32(reader["ID_Proveedores"]);
                    lista.Add(P);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Proveedores " + ex.Message);
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
        /// Metodo que permite retornar todos los proveedores de una id similar al BienServicio a buscar
        /// </summary>
        /// <param name="idBienServicio"></param>
        /// <returns></returns>
      
        public async Task<IEnumerable<Proveedores>> GetAllProveedoresBienServicio(int id)
        {

            List<Proveedores> lista = new List<Proveedores>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT P.*, BS.Bien_Servicio " +
                    "FROM dbo.Proveedores P " +
                    "INNER JOIN dbo.Bien_Servicio BS ON BS.ID_Bien_Servicio = P.ID_Bien_Servicio " +
                    "where P.ID_Bien_Servicio = @id"; // leer base datos 
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@id", SqlDbType.Int).Value = id;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Proveedores P = new();
                    P.Rut_Proveedor = (Convert.ToString(reader["Rut_Proveedor"])).Trim();
                    P.Razon_Social = (Convert.ToString(reader["Razon_social"])).Trim();
                    P.Nombre_Fantasia = (Convert.ToString(reader["Nombre_Fantasia"])).Trim();
                    P.ID_Bien_Servicio = Convert.ToString(reader["Bien_Servicio"]).Trim();
                    P.Direccion = (Convert.ToString(reader["Direccion"])).Trim();
                    P.Comuna = (Convert.ToString(reader["Comuna"])).Trim();
                    P.Correo_Proveedor = (Convert.ToString(reader["Correo_Proveedor"])).Trim();
                    P.Telefono_Proveedor = Convert.ToString(reader["Telefono_Proveedor"]).Trim();
                    P.Cargo_Representante = (Convert.ToString(reader["Cargo_Representante"])).Trim();
                    P.Nombre_Representante = (Convert.ToString(reader["Nombre_Representante"])).Trim();
                    P.Email_Representante = (Convert.ToString(reader["Email_Representante"])).Trim();
                    P.Estado = Convert.ToBoolean(reader["Estado"]);
                    P.N_Cuenta = Convert.ToString(reader["N_Cuenta"]).Trim();
                    P.Banco = (Convert.ToString(reader["Banco"])).Trim();
                    P.Swift = (Convert.ToString(reader["Swift"])).Trim();
                    P.ID_Proveedores = Convert.ToInt32(reader["ID_Proveedores"]);
                    P.Cargo_Representante = Convert.ToString(reader["Cargo_Representante"]).Trim();
                    lista.Add(P);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Proveedores " + ex.Message);
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
        /// Metodo que "elimina" al proveedor, con confundir con Estado
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Proveedores> EliminarProveedor(int P)
        {
            
            
        Proveedores Pmod = null;
        SqlConnection sqlConexion = conectar();
        SqlCommand? Comm = null;
        SqlDataReader reader = null;
        try
        {
            sqlConexion.Open();
            Comm = sqlConexion.CreateCommand();
            Comm.CommandText = "UPDATE dbo.Proveedores SET " +
                "Estado = @Activado " +
                "WHERE ID_Proveedores = @ID_Proveedores";
            Comm.CommandType = CommandType.Text;
            Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = false;
            Comm.Parameters.Add("@ID_Proveedores", SqlDbType.Int).Value = P;


            reader = await Comm.ExecuteReaderAsync();
            if (reader.Read())
                Pmod = await GetProveedor(Convert.ToInt32(reader["ID_Proveedores"]));
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
        return Pmod;
    }



        /// <summary>
        /// Metodo para asegurar que el rut sea valido
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        public bool CalcularDigitoVerificador(string rut)
        {
            string rutSinDV = rut.Replace(".", "").Replace("-", "").Replace(" ", "").ToUpper();

            string digito = rutSinDV.Substring(rutSinDV.Length - 1).ToUpper();

            rutSinDV = rutSinDV.Substring(0, rutSinDV.Length - 1);

            if (rutSinDV.Length < 7 || rutSinDV.Length > 9)
                return false;
            else if (!Regex.IsMatch(rutSinDV, @"^[0-9]+$"))
            {
                return false;
            }
            int suma = 0;
            int multiplicador = 2;

            // Iterar sobre los dígitos del RUT de derecha a izquierda
            for (int i = rutSinDV.Length - 1; i >= 0; i--)
            {
                suma += (int)char.GetNumericValue(rutSinDV[i]) * multiplicador;
                multiplicador = multiplicador == 7 ? 2 : multiplicador + 1;
            }

            int resto = 11 - (suma % 11);

            // Si el resto es 11, el dígito verificador es 0. Si es 10, es 'K'.
            if (resto == 11)
            {
                return digito.Equals("0");
            }

            else if (resto == 10) return digito.Equals("K");
            else return digito.Equals(resto.ToString());
        }
        /// <summary>
        /// Metodo para normalizar los rut
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        public string RutNormalizado(string rut)
        {
            string rutInvertido = new string(rut.Reverse().ToArray());

            // Insertar puntos cada 3 dígitos
            string rutFormateado = "";
            for (int i = 0; i < rutInvertido.Length; i++)
            {
                rutFormateado += rutInvertido[i];
                if (i % 3 == 0 && i != 0)
                {
                    rutFormateado += ".";
                }
            }

            // Invertir nuevamente y agregar el guión
            rutFormateado = new string(rutFormateado.Reverse().ToArray());
            rutFormateado = rutFormateado.Insert(rutFormateado.Length - 1, "-");

            return rutFormateado;
        }


    }
}