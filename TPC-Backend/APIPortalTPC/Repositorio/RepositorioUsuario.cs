using APIPortalTPC.Datos;              
using System.Data.SqlClient;
using System.Data;
using ClasesBaseDatosTPC;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using BCrypt.Net;
using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioUsuario : IRepositorioUsuario
    {

        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioUsuario(AccesoDatos CD)
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
        /// Se crea  un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="id">Id del Objeto Usuario a buscar</param>
        /// <returns>Retorna el objeto Usuario agregado a la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Usuario> NuevoUsuario(Usuario U)
        {
            string rut= U.Rut_Usuario.Replace(".", "").Replace("-", "").Replace(" ", "").ToUpper(); 
            if (!CalcularDigitoVerificador(rut))
            {
                throw new Exception("Error de Rut ");
            }
            else
            {


                SqlConnection sql = conectar();
                SqlCommand? Comm = null; ;
                try
                {
                    sql.Open();
                    Comm = sql.CreateCommand();
                    Comm.CommandText = "INSERT INTO Usuario" +
                        "(Nombre_Usuario, Apellido_Paterno, Rut_Usuario, Apellido_Materno, Correo_Usuario,  Tipo_Liberador, En_Vacaciones, Admin, Activado) " +
                        "VALUES (@Nombre_Usuario, @Apellido_Paterno, @Rut_Usuario, @Apellido_Materno, @Correo_Usuario, @Tipo_Liberador, @En_Vacaciones,@Admin,@Activado) " +
                        "SELECT SCOPE_IDENTITY() AS Id_Usuario";
                    Comm.CommandType = CommandType.Text;
                    Comm.Parameters.Add("@Nombre_Usuario", SqlDbType.VarChar, 50).Value = U.Nombre_Usuario;
                    Comm.Parameters.Add("@Apellido_Paterno", SqlDbType.VarChar, 50).Value = U.Apellido_paterno;
                    Comm.Parameters.Add("@Rut_Usuario", SqlDbType.VarChar, 50).Value = RutNormalizado(rut);
                    Comm.Parameters.Add("@Apellido_Materno", SqlDbType.VarChar, 50).Value = U.Apellido_materno;
                    Comm.Parameters.Add("@Correo_Usuario", SqlDbType.VarChar, 100).Value = U.Correo_Usuario;
                    Comm.Parameters.Add("@En_Vacaciones", SqlDbType.Bit).Value = U.En_Vacaciones;
                    Comm.Parameters.Add("@Admin", SqlDbType.Bit).Value = U.Admin;
                    Comm.Parameters.Add("@Tipo_Liberador", SqlDbType.Bit).Value = U.Tipo_Liberador;
                    Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = false;
   
                    decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                    int id = (int)idDecimal;
                    U.Id_Usuario = id;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error creando los datos en tabla Usuario " + ex.Message);
                }
                finally
                {
                    Comm?.Dispose();
                    sql.Close();
                    sql.Dispose();
                }
                return U;
            }
        }

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna el Usuario buscado por la Id dada</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Usuario> GetUsuario(int? id)
        {
            //Parametro para guardar el objeto a mostrar
            Usuario U = new();
            List<string> listaDep = new List<string>();
            List<int> IdListaDep = new List<int>();
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
                Comm.CommandText = "SELECT * " +
                   "FROM dbo.Usuario  " +
                   "WHERE Id_Usuario = @Id_Usuario";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    U.Nombre_Usuario = (Convert.ToString(reader["Nombre_Usuario"])).Trim();
                    U.Apellido_paterno = (Convert.ToString(reader["Apellido_Paterno"])).Trim();
                    U.Apellido_materno = (Convert.ToString(reader["Apellido_Materno"])).Trim();
                    U.Correo_Usuario = (Convert.ToString(reader["Correo_Usuario"])).Trim();
                    U.Contraseña_Usuario = (Convert.ToString(reader["Contraseña_Usuario"])).Trim();
                    U.Tipo_Liberador = (Convert.ToBoolean(reader["Tipo_Liberador"]));
                    U.En_Vacaciones = Convert.ToBoolean(reader["En_Vacaciones"]);
                    U.Rut_Usuario = Convert.ToString(reader["Rut_Usuario"]).Trim();
                    U.Activado = Convert.ToBoolean(reader["Activado"]);
                    U.Admin = Convert.ToBoolean(reader["Admin"]);
                    U.CodigoMFA = Convert.ToInt32(reader["CodigoMFA"]);
                    U.Nombre_Completo = U.Nombre_Usuario + " " + U.Apellido_materno + " " + U.Apellido_paterno;
                    U.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);

                }
                reader?.Close();
                Comm?.Dispose();
                Comm.CommandText = "SELECT d.nombre, d.Id_Departamento " +
                    "FROM Departamento d JOIN DepartamentoUsuario du ON d.Id_Departamento = du.Id_Departamento " +
                    "WHERE du.Id_Usuario = @Id_Usuario";
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read()) {
                    int c = 0;
                    string dep = Convert.ToString(reader["nombre"]).Trim();
                    int idep = Convert.ToInt32(reader["Id_Departamento"]);
                foreach (string d in listaDep)
                    if (d == dep)
                         c = 1;
                    if (c == 0)
                    {
                        listaDep.Add(dep);
                        IdListaDep.Add(idep);
                    }
                }
                U.ListaDepartamento = listaDep;
                U.ListaIdDep = IdListaDep;

            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Usuario " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader?.Close();
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return U;
        }

        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna la lista de objetos Usuarios de la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Usuario>> GetAllUsuario()
        {
            List<Usuario> lista = new List<Usuario>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Usuario Where Activado=@A"; // leer base datos 
                Comm.CommandType = CommandType.Text;

                Comm.Parameters.Add("@A", SqlDbType.Bit).Value = true;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Usuario U = new();
                    U.Nombre_Usuario = (Convert.ToString(reader["Nombre_Usuario"])).Trim();
                    U.Apellido_paterno = (Convert.ToString(reader["Apellido_Paterno"])).Trim();
                    U.Apellido_materno = (Convert.ToString(reader["Apellido_Materno"])).Trim();
                    U.Correo_Usuario = (Convert.ToString(reader["Correo_Usuario"])).Trim();
                    U.Contraseña_Usuario = (Convert.ToString(reader["Contraseña_Usuario"])).Trim();
                    U.Tipo_Liberador = (Convert.ToBoolean(reader["Tipo_Liberador"]));
                    U.En_Vacaciones = Convert.ToBoolean(reader["En_Vacaciones"]);
                    U.Rut_Usuario = Convert.ToString(reader["Rut_Usuario"]).Trim();
                    U.Activado = Convert.ToBoolean(reader["Activado"]);
                    U.Admin = Convert.ToBoolean(reader["Admin"]);
                    U.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);
                    U.Nombre_Completo = U.Nombre_Usuario + " " + U.Apellido_materno + " " + U.Apellido_paterno;
                    U.CodigoMFA = Convert.ToInt32(reader["CodigoMFA"]);
                    lista.Add(U);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Usuario "+ ex.Message);
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
        /// Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="U">Objeto Usuario que se usa para reemplazar su homonimo dentro de la base de datos</param>
        /// <returns>Retorna el objeto Usuario modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Usuario> ModificarUsuario(Usuario U)
        {
            string rut = U.Rut_Usuario.Replace(".", "").Replace("-", "").Replace(" ", "").ToUpper();
            
            if (!CalcularDigitoVerificador(rut))
                throw new Exception("Error de Rut ");
            Usuario? Umod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Usuario SET " +
                                    "Nombre_Usuario = @Nombre_Usuario, " +
                                    "Apellido_Paterno = @Apellido_Paterno, " +
                                    "Rut_Usuario = @Rut_Usuario, " +
                                    "Apellido_Materno = @Apellido_Materno, " +
                                    "Correo_Usuario = @Correo_Usuario, " +
                                    "Contraseña_Usuario = @Contraseña_Usuario, " +
                                    "Tipo_Liberador = @Tipo_Liberador, " +
                                    "En_Vacaciones = @En_Vacaciones, " +
                                    "CodigoMFA = @CodigoMFA, " +
                                    "Activado =@Activado, " +
                                    "Admin = @Admin " +
                                    " WHERE  Id_Usuario = @Id_Usuario;";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Nombre_Usuario", SqlDbType.VarChar, 50).Value = U.Nombre_Usuario;
                Comm.Parameters.Add("@Apellido_Paterno", SqlDbType.VarChar, 50).Value = U.Apellido_paterno;
                Comm.Parameters.Add("@Rut_Usuario", SqlDbType.VarChar,50).Value = RutNormalizado(rut);
                Comm.Parameters.Add("@Apellido_Materno", SqlDbType.VarChar, 50).Value = U.Apellido_materno;
                Comm.Parameters.Add("@Correo_Usuario", SqlDbType.VarChar, 50).Value = U.Correo_Usuario;
               
                Comm.Parameters.Add("@Tipo_Liberador", SqlDbType.Bit).Value = U.Tipo_Liberador;
                Comm.Parameters.Add("@En_Vacaciones", SqlDbType.Bit).Value = U.En_Vacaciones;
                Comm.Parameters.Add("@Admin", SqlDbType.Bit).Value = U.Admin;
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = U.Activado;
                if(U.CodigoMFA==1)
                    Comm.Parameters.Add("@Contraseña_Usuario", SqlDbType.VarChar, 100).Value = BCrypt.Net.BCrypt.HashPassword(U.Contraseña_Usuario);
                else
                    if (U.Contraseña_Usuario == U.CodigoMFA.ToString())

                      Comm.Parameters.Add("@Contraseña_Usuario", SqlDbType.VarChar, 100).Value = BCrypt.Net.BCrypt.HashPassword(U.Contraseña_Usuario);
                else
                            Comm.Parameters.Add("@Contraseña_Usuario", SqlDbType.VarChar, 100).Value = (U.Contraseña_Usuario);
               

                if (U.CodigoMFA != 1)
                {
                    Comm.Parameters.Add("@CodigoMFA", SqlDbType.Int).Value = U.CodigoMFA;
                }
                else
                {
                    Comm.Parameters.Add("@CodigoMFA", SqlDbType.Int).Value = 0;

                }
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = U.Id_Usuario;
                
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Umod = await GetUsuario(Convert.ToInt32(reader["Id_Usuario"]));

              
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando del Usuario " + ex.Message);
            }
            finally
            {
                reader?.Close();

                Comm?.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Umod;
        }


        /// <summary>
        /// Metodo para revisar si existe un Usuario Unico
        /// </summary>
        /// <param name="rut">Rut a buscar</param>
        /// <param name="correo">Correo a buscar</param>
        /// <returns></returns>
        public async Task<string> Existe(string rut, string correo)
        {
            rut=rut.Replace(".", "").Replace("-", "").Replace(" ", "").ToUpper();
            rut = RutNormalizado(rut);
            if (!CalcularDigitoVerificador(rut))
            {
                return "El rut no es valido";
            }
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Usuario WHERE Rut_Usuario = @rut and Activado = @A";
                    command.Parameters.AddWithValue("@rut", rut);
                    command.Parameters.AddWithValue("@A", true);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        return "El rut ya existe";
                    }
                    reader.Close();
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Usuario WHERE Correo_Usuario = @correo and Activado = @Ac";
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@Ac", true);
                    reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        return "El correo ya existe";
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
        /// muestra todos los usuarios que son liberadores
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Usuario>> GetAllUsuariosLiberadores()
        {
            List<Usuario> lista = new List<Usuario>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = @"select  U.* 
                from dbo.Departamento D 
                inner join dbo.Usuario U on D.Encargado = U.Id_Usuario 
                inner join dbo.Ticket T on T.Id_Usuario = U.Id_Usuario 
                where lower(T.Estado) != 'liberación concluida' and Lower(U.Tipo_Liberador) != 'no' and U.Activado != 0"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Usuario U = new();
                    U.Nombre_Usuario = (Convert.ToString(reader["Nombre_Usuario"])).Trim();
                    U.Apellido_paterno = (Convert.ToString(reader["Apellido_Paterno"])).Trim();
                    U.Apellido_materno = (Convert.ToString(reader["Apellido_Materno"])).Trim();
                    U.Correo_Usuario = Convert.ToString(reader["Correo_Usuario"]).Trim();
                    U.Contraseña_Usuario = Convert.ToString(reader["Contraseña_Usuario"]).Trim();
                    U.Tipo_Liberador = Convert.ToBoolean(reader["Tipo_Liberador"]);
                    U.En_Vacaciones = Convert.ToBoolean(reader["En_Vacaciones"]);
                    U.Rut_Usuario = Convert.ToString(reader["Rut_Usuario"]).Trim();
                    U.Activado = Convert.ToBoolean(reader["Activado"]);
                    U.Admin = Convert.ToBoolean(reader["Admin"]);
                    U.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);
                    lista.Add(U);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Usuario " + "aaa" + ex.Message);
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
    /// Metodo que busca por Id el Usuario para "eliminarlo"
    /// </summary>
    /// <param name="U"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
        public async Task<Usuario> EliminarUsuario(int U)
        {
            Usuario? Umod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Usuario SET " +
                                    "Activado = @Activado " +
                                    "WHERE  Id_Usuario = @Id_Usuario";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = false;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = U;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Umod = await GetUsuario(Convert.ToInt32(reader["Id_Usuario"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando del Usuario " + ex.Message);
            }
            finally
            {
                reader?.Close();

                Comm?.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Umod;
        }
        
        /// <summary>
        /// Metodo que permite que el Usuario pueda registarse en la plataforma
        /// </summary>
        /// <param name="U"></param>
        /// <returns></returns>
        public async Task<Usuario> ActivarUsuario(Usuario U)
        {
            Random random = new Random();
            int pass = random.Next(100000, 999999);
            U.Contraseña_Usuario = pass.ToString();
            U.CodigoMFA = pass;
            return U;
        }
        /// <summary>
        /// Metodo que retorna la contraseña del usuario que puso su correo
        /// </summary>
        /// <param name="correo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Usuario> RecuperarContraseña(string correo)
        {
            Usuario U = new ();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Usuario " +
                   "WHERE Correo_Usuario = @correo and Activado = @bool";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@correo", SqlDbType.VarChar, 500).Value = correo;
                Comm.Parameters.Add("@bool", SqlDbType.Bit).Value = true;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    U.Nombre_Usuario = (Convert.ToString(reader["Nombre_Usuario"])).Trim();
                    U.Apellido_paterno = (Convert.ToString(reader["Apellido_Paterno"])).Trim();
                    U.Apellido_materno = (Convert.ToString(reader["Apellido_Materno"])).Trim();
                    U.Correo_Usuario = (Convert.ToString(reader["Correo_Usuario"])).Trim();
                    U.Contraseña_Usuario = (Convert.ToString(reader["Contraseña_Usuario"])).Trim();
                    U.Tipo_Liberador = (Convert.ToBoolean(reader["Tipo_Liberador"]));
                    U.En_Vacaciones = Convert.ToBoolean(reader["En_Vacaciones"]);
                    U.Rut_Usuario = Convert.ToString(reader["Rut_Usuario"]).Trim();
                    U.Activado = Convert.ToBoolean(reader["Activado"]);
                    U.Admin = Convert.ToBoolean(reader["Admin"]);
                    U.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);
                    U.Nombre_Completo = U.Nombre_Usuario + " " + U.Apellido_materno + " " + U.Apellido_paterno;
                    U.CodigoMFA = 1;

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Usuario " + ex.Message);
            }
            finally
            {
                reader?.Close();
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return U;

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