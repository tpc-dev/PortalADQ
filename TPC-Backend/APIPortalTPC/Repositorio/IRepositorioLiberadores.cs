using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioLiberadores
    {
        public Task<Liberadores> Nuevo(Liberadores L);
        public Task<Liberadores> Get(int id);
        public Task<IEnumerable<Liberadores>> GetAll();
        public Task<Liberadores> Modificar(Liberadores L);
        public Task<string> Existe(int Usuario, int Dep);
        public Task<Liberadores> GetDep(int dep);
    }
}
