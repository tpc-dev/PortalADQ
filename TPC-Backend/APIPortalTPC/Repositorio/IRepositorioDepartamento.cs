using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioDepartamento
    {
        public Task<Departamento> NuevoDepartamento(Departamento D);
        public Task<Departamento> GetDepartamento(int id);
        public Task<IEnumerable<Departamento>> GetAllDepartamento();
        public Task<Departamento> ModificarDepartamento(Departamento D);
        public Task<Departamento> EliminarDepartamento(int D);
        public Task<string> Existe(string nombre);
    }
}
