using BaseDatosTPC;
namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioReemplazos
    {
        public Task<Reemplazos> NuevoReemplazos(Reemplazos R);
        public Task<Reemplazos> GetReemplazo(int id);
        public Task<IEnumerable<Reemplazos>> GetAllRemplazos();
        public Task<Reemplazos> ModificarReemplazos(Reemplazos R);
    }
}
