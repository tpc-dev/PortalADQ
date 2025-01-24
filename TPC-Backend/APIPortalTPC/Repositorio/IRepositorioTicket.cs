using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioTicket
    {
        public Task<Ticket> NewTicket(Ticket T);
        public Task<Ticket> GetTicket(int id);
        public Task<IEnumerable<Ticket>> GetAllTicket();
        public Task<Ticket> ModificarTicket(Ticket T);
        public Task<Ticket> ActualizarEstadoTicket(int id_T);
        public Task<Ticket> EliminarTicket(int id_T);
        public Task<IEnumerable<Ticket>> GetAllTicketUsuario(int id);
        public Task<IEnumerable<int>> TicketConOCPendientes(int id_U);
        public Task<Ticket> GetTicketOC(int id);
    }
}
