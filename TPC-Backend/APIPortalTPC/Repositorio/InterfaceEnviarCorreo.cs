using BaseDatosTPC;
using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    /// <summary>
    /// Interface con metodos que envian especificos correos despues de cumplir ciertas condiciones
    /// </summary>
    public interface InterfaceEnviarCorreo
    {
        public Task<string> CorreoProveedores(Proveedores P, FormData formData);
        public Task<string> CorreoLiberador(Usuario U, string subject);
        public Task<string> CorreoRecepciones(Usuario U, string subject, int Id_Ticket);
        public Task<string> RecuperarPass(Usuario U);
        public Task<string> CorreoUsuarioPass(Usuario U);

    }
}
