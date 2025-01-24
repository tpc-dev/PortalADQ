using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioCentroCosto
    {
        public Task<CentroCosto> Nuevo_CeCo(CentroCosto Ceco);
        public Task<CentroCosto> GetCeCo(int  IdCECO);
        public Task<IEnumerable<CentroCosto>> GetAllCeCo();
        public Task<CentroCosto> ModificarCeCo(CentroCosto CeCo);
        public Task<CentroCosto> EliminarCeCo(int CeCo);
        public Task<string> Existe(string Ceco);
        public Task<CentroCosto> GetCeCo(string code);
    }
}

