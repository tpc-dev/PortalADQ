using BaseDatosTPC;

/*
 * Interface de los metodos para obtener los datos de la taba Bien_Servicio de la base de datos
 * */
namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioBienServicio
    {
        public Task<BienServicio> NuevoBienServicio(BienServicio bs);
        public Task<BienServicio> GetServicio(int id);
        public Task<IEnumerable<BienServicio>> GetAllServicio();
        public Task<BienServicio> ModificarBien_Servicio(BienServicio bs);
        public Task<string> Existe(string BienServicio);
        public Task<BienServicio> EliminarBien_Servicio(int bs);
        public Task<BienServicio> GetServicioNombre(string bsn);
    }
}
