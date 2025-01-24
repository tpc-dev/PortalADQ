using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class Ticket
    {
        [Key]
        public int ID_Ticket { get; set; }

        public string? Estado { get; set; }

        public DateTime Fecha_Creacion_OC { get; set; }

        public string? Id_Usuario { get; set; }

        public int Id_U { get; set; }

        public string? ID_Proveedor { get; set; }


        public string? Detalle { get; set; }

        public long? Solped { get; set; }
        
        public string? Id_OE { get; set; }
        public int N_OE { get; set; }
      
        public long Numero_OC { get; set; }
        public DateTime? Fecha_OC_Recepcionada { get; set; }

        public DateTime? Fecha_OC_Enviada { get; set; }

        public DateTime? Fecha_OC_Liberada { get; set; }

        public bool Activado { get; set; }

    }
}
