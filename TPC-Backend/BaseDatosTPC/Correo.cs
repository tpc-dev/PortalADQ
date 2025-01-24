using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    /// <summary>
    /// Clase correo guarda los datos que se mostraran, pero los datos que se van a guardar en la base de datos son menos, 
    /// esto es para evitar que esta clase este tan arraigada a varias tablas 
    /// </summary>
    public class Correo
    {
        public int Id_Correo { get; set; }
        public int Id_Ticket { get; set; }
        public string Solicitante { get; set; }
        public int ID_Solicitante { get; set; }
        public string Proveedor { get; set; }
        public string CeCo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CorreosEnviados { get; set; }
        public DateTime? PrimerCorreo { get; set; }
        public DateTime? UltimoCorreo { get; set; }
        public string? detalle { get; set; }    
        public long Numero_OC { get; set; }
        public bool Activado { get; set; }

    }
}
