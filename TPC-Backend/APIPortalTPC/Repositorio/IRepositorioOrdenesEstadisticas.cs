using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioOrdenesEstadisticas
    {
        public Task<OrdenesEstadisticas> NuevoOE(OrdenesEstadisticas OE);
        public Task<OrdenesEstadisticas> GetOE(int id);
        public Task<IEnumerable<OrdenesEstadisticas>> GetAllOE();
        public Task<OrdenesEstadisticas> ModificarOE(OrdenesEstadisticas OE);
        public Task<OrdenesEstadisticas> EliminarOE(int OE);

        public Task<string> Existe(string code);
        public Task<OrdenesEstadisticas> GetOECode(string id);
    }
}
