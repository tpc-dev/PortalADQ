using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioRelacion
    {
        public Task<Relacion> NuevaRelacion(Relacion R);
        public Task<Relacion> GetRelacion(int id);
        public Task<IEnumerable<Relacion>> GetAllRelacion();
        public Task<Relacion> ModificarRelacion(Relacion R);
    }
}
