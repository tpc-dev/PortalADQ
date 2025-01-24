using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Archivo_Ticket
    {
        public int ID_Ticket { get; set; }

        public string? Estado { get; set; }

        public DateTime Fecha_Creacion_OC { get; set; }

        public string? Id_Usuario { get; set; }

        public int Id_U { get; set; }

        public string? ID_Proveedor { get; set; }


        public string? Detalle { get; set; } = " ";

        public long? Solped { get; set; } = 0;

        public string? Id_OE { get; set; }
        public int N_OE { get; set; }

        public long Numero_OC { get; set; }
        public DateTime? Fecha_OC_Recepcionada { get; set; }

        public DateTime? Fecha_OC_Enviada { get; set; }

        public DateTime? Fecha_OC_Liberada { get; set; }

        public bool Activado { get; set; }
        public IFormFile file { get; set; }

        public string fileName { get; set; }
    }
}
