using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    /// <summary>
    /// Clase Relacion une la clase Ticket a una clase Archivo o una clase Cotizacion a una clase Archivo
    /// Esto se usa para buscar los archivos relacionados
    /// </summary>
    public class Relacion
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int Id_Relacion { get; set; }
        /// <summary>
        /// Identificador del archivo asociado
        /// </summary>
        public int? Id_Archivo { get; set; }
        /// <summary>
        /// Identificador del Ticket asociado
        /// </summary>
        public int? Id_Cotizacion { get; set; }
        /// <summary>
        /// Identificador de la cotizacion
        /// </summary>

    }
}
