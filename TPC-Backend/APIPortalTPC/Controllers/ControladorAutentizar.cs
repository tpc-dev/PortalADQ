using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorAutentizar : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioAutentizar RA;
        private readonly IRepositorioUsuario RU;
        private readonly InterfaceEnviarCorreo IEC;
  
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RA">Interface de RepositorioUsuario</param>
        public ControladorAutentizar(InterfaceEnviarCorreo IEC, IRepositorioAutentizar RA, IRepositorioUsuario RU)
        {
            this.RA = RA;
            this.RU = RU;
            this.IEC = IEC;
        }
        /// <summary>
        ///  Recibe una clase que contiene el correo electrónico y la contraseña para validar su existencia, 
        ///  tambien añade la clave MFA para
        ///  poder ser usada para confirmar
        /// </summary>
        /// <param name="postrq">Objeto que guarda el correo y contraseña a comprobar>
        /// <returns>User es el usuario confirmado </returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> ValidarCorreo(PostRq postrq)
        {
            Usuario User = await RA.ValidarCorreo(postrq.correo, postrq.pass);

            if (User.Id_Usuario == 0)
            {
                return NotFound("Clave o contraseña incorrecta");
            }
            bool activado = User.Activado;

            if (activado)
            {
                int codigo = await RA.MFA(User.Correo_Usuario);
                User.CodigoMFA = codigo;
                await RU.ModificarUsuario(User);
            return User;
            }
            return NotFound("Usuario no activado");

        }
        [HttpPost("MFA")]

        /// <summary>
        /// metodo que se encarga de comprobar su la contraseña ingresada es valida, para eso necesita la id del Usuario que
        /// se quiere comprobar
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public async Task<ActionResult<Usuario>> MFA1(MFA mfa)
        {
            int id = mfa.Id_Usuario;
            Usuario U = await RU.GetUsuario(id);
            if(U.CodigoMFA== mfa.mfa)
            {
                U.CodigoMFA = 0;
                RU.ModificarUsuario(U);
        
                return U;
            }
                
            else
            {
                return NotFound("Codigo Incorrecto");
            }
        }
        /// <summary>
        /// Metodo que valida usuario nuevo
        /// </summary>
        /// <param name="postrq"></param>
        /// <returns></returns>
        [HttpPost("nuevo")]
        public async Task<ActionResult<Usuario>> NuevoUsuario(PostRq postrq)
        {
            Usuario User = await RA.ValidarCorreo(postrq.correo, postrq.pass);

            if (User.Id_Usuario == 0)
            {
                return NotFound("Clave o contraseña incorrecta");
            }
            bool activado = User.Activado;
            
            if (!activado)
            {

                if (User.Contraseña_Usuario == User.CodigoMFA.ToString())
                    User.CodigoMFA = 0;
                    User.Activado = true;
                    await RU.ModificarUsuario(User);
                    //despues de esto llamas al metodo del controlador Usuario para modificar el usuario
                    return User;
            }
            return NotFound("El usuario ya esta activado");
        }


        /// <summary>
        /// Metodo que envia un correo con la contraseña del usuario</summary>
        /// <param name="Correo"></param>
        /// <returns></returns>
        [HttpPost("pass")]
        public async Task<IActionResult> RecuperarContraseña(PostRq Correo)
        {
            try
            { //Logica: entregas el correo,
              //retorna el usuario
                Usuario U = await RU.RecuperarContraseña(Correo.correo);

                //mandas un correo,
                //luego mandas un correo para confirmar y ahi se hace el cambio de contraseña
                if (U != null)
                {
                    Random random = new Random();
                    int newpass = random.Next(100000, 999999);
                    U.Contraseña_Usuario = newpass.ToString();
                    U.CodigoMFA = 1;
                    await IEC.RecuperarPass(U);
                    U = await RU.ModificarUsuario(U);
   
                    return Ok(U);
                }
                return BadRequest("No hay usuario para ese correo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
