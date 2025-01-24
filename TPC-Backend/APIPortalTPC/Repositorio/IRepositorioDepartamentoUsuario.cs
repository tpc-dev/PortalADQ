using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioDepartamentoUsuario
    {
        public Task<DepartamentoUsuario> Nuevo(DepartamentoUsuario DU);
        public Task<DepartamentoUsuario> Get(int id);
        public Task<IEnumerable<DepartamentoUsuario>> GetAll();
        public Task<DepartamentoUsuario> Modificar(DepartamentoUsuario DU);
        public Task<string> Existe(string Usuario, string Dep);
    }
}
