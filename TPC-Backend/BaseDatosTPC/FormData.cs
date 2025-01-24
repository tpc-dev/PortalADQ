

using Microsoft.AspNetCore.Http;

namespace BaseDatosTPC
{
    public class FormData
    {
        public IFormFile? file { get; set; }
        public string? Asunto { get; set; }
        public int? iD_Bien_Servicio { get; set; }
        public string? Mensaje { get; set; }
        public string? Proveedor { get; set; }
        public string? Id_Cotizacion { get; set; } = "0";

    }
}
