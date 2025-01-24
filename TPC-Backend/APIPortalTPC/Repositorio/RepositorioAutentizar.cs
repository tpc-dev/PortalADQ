using APIPortalTPC.Datos;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Security.Policy;


namespace APIPortalTPC.Repositorio
{
    public class RepositorioAutentizar : IRepositorioAutentizar
    {
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioAutentizar(AccesoDatos CD)
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
        /// Retorna el Usuario validado si su contraseña y contraseña coinciden
        /// </summary>
        /// <param name="correo">Correo a buscar</param>
        /// <param name="pass">Contraseña a buscar</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Usuario> ValidarCorreo(string correo, string pass)
        {
            Usuario U = new();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;

            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Usuario " +
                                   "WHERE Correo_Usuario = @correo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.AddWithValue("@correo", correo);

                reader = await Comm.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        U.Nombre_Usuario = (Convert.ToString(reader["Nombre_Usuario"])).Trim();
                        U.Apellido_paterno = (Convert.ToString(reader["Apellido_Paterno"])).Trim();
                        U.Apellido_materno = (Convert.ToString(reader["Apellido_Materno"])).Trim();
                        U.Correo_Usuario = (Convert.ToString(reader["Correo_Usuario"])).Trim();
                        U.Contraseña_Usuario = (Convert.ToString(reader["Contraseña_Usuario"])).Trim(); 
                        U.Tipo_Liberador = Convert.ToBoolean(reader["Tipo_Liberador"]);
                        U.En_Vacaciones = Convert.ToBoolean(reader["En_Vacaciones"]);
                        U.Rut_Usuario = Convert.ToString(reader["Rut_Usuario"]).Trim();
                        U.Activado = Convert.ToBoolean(reader["Activado"]);
                        U.Admin = Convert.ToBoolean(reader["Admin"]);
                        U.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);
                        U.CodigoMFA = Convert.ToInt32(reader["CodigoMFA"]);
                    }
                }
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
            if (BCrypt.Net.BCrypt.Verify(pass,U.Contraseña_Usuario)) return U;


            //else
                return new Usuario();
        }

        /// <summary>
        /// Metodo que crea el numero aleatorio y manda el correo al usuario
        /// </summary>
        /// <param name="correo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> MFA(string correo)
        {
            Random random = new Random();
            int codigoVerificacion = random.Next(100000, 999999);
            string smtpServer = " tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP
            int smtpPort = 25; // Cambia el puerto si es necesario
            string fromEmail = "portaladquisiones@tpc.cl"; // Cambia esto a tu correo
            string subject = "Código de Verificación - Autenticación en dos pasos";
            // Crear el cuerpo del mensaje
            string body = $@"
            <html>
                <head>
                    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                    <style>
                        .numero-grande {{
                            font-size: 50px;
                            font-weight: bold;
                            text-align: center;
                            color: #333;
                        }}
                    </style>
                </head>
                <body>
                    <p>Estimado,</p>
                    <p>Su código de verificación es: </p>
                    <p class='numero-grande'>{codigoVerificacion}</p>
                    <p>Saludos cordiales,</p>
                    <p>Equipo de Soporte</p>
                </body>
            </html>";
           
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(correo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.EnableSsl = true; // Habilitar SSL si es necesario
                        smtpClient.Send(mail);
                    }
                }

                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar el correo: {ex.Message}");
            }
            return codigoVerificacion;
        }
    

    }
}
