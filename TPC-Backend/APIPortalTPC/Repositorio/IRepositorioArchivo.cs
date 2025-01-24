using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioArchivo
    {
        public Task<Archivo> NuevoArchivo(Archivo A);
        public Task<Archivo> GetArchivo(int id);
        public Task<IEnumerable<Archivo>> GetAllArchivo();
        public Task<Archivo> ModificarArchivo(Archivo A);
 
    }
}
