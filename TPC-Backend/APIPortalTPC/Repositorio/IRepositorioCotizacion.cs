using BaseDatosTPC;
namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioCotizacion
    {
        public Task<Cotizacion> NuevaCotizacion(Cotizacion cotizacion); 
        public Task<Cotizacion> GetCotizacion(int id);
        public Task <IEnumerable<Cotizacion>> GetAllCotizacion();
        public Task<Cotizacion> ModificarCotizacion(Cotizacion cotizacion);
        public Task<Cotizacion> EliminarCotizacion(int cotizacion);

        
    }
}