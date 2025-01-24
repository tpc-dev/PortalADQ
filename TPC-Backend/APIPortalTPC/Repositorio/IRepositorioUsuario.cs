using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioUsuario
    {
        public Task<Usuario> NuevoUsuario(Usuario U);
        public Task<Usuario> GetUsuario(int? id );
        public Task<IEnumerable<Usuario>> GetAllUsuario();
        public Task<Usuario> ModificarUsuario(Usuario U);
        public Task<string> Existe(string rut, string correo );
        public Task<Usuario> EliminarUsuario(int U);
        public Task<Usuario> ActivarUsuario(Usuario U);
        public Task<Usuario> RecuperarContraseña(string correo);

    }
}
