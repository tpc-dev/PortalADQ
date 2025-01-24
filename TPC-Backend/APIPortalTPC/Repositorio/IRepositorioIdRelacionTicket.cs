using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioIdRelacionTicket
    {
    public  Task<Id_RelacionTicket> GetRelacion(int id);
    public  Task<Id_RelacionTicket> NuevaRelacion(Id_RelacionTicket R);
    public  Task<IEnumerable<Id_RelacionTicket>> GetAllRelacion();
    public  Task<Id_RelacionTicket> ModificarRelacion(Id_RelacionTicket R);
    }
}
