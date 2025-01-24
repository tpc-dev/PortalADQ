using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioOrdenCompra
    {
        public Task<OrdenCompra> NuevoOC(OrdenCompra OC);
        public Task<OrdenCompra> GetOC(int id);
        public Task<IEnumerable<OrdenCompra>> GetAllOC();
        public Task<OrdenCompra> ModificarOC(OrdenCompra OC);
        public Task<OrdenCompra> EliminarOC(int OC);
        public  Task<IEnumerable<OrdenCompra>> GetAllOCTicket(int id_T);
        public Task<string> Existe(long Numero_OC,string posicion);
    }
}
