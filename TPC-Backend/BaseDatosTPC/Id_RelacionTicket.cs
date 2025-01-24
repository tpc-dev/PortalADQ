using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Id_RelacionTicket
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int IdRelacionTicket { get; set; }
        /// <summary>
        /// Identificador del archivo asociado
        /// </summary>
        public int? Id_Archivo { get; set; }
        /// <summary>
        /// Identificador del Ticket asociado
        /// </summary>
        public int? Id_Ticket { get; set; }
        /// <summary>
        /// Identificador de la cotizacion
        /// </summary>
    }
}
