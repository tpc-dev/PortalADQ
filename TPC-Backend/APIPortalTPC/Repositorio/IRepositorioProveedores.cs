using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioProveedores
    {
        public Task<Proveedores> NuevoProveedor(Proveedores P);
        public Task<Proveedores> GetProveedor(int id);
        public Task<IEnumerable<Proveedores>> GetAllProveedores();
        public Task<Proveedores> ModificarProveedor(Proveedores P);
        public Task<Proveedores> EliminarProveedor(int P);
        public Task<string> Existe(string rut, string bs);
        public Task<IEnumerable<Proveedores>> GetAllProveedoresBienServicio(int idBienServicio);
    }
}
