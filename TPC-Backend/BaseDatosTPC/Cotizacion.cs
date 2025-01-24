using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    /// <summary>
    /// Guarda todas las cotizaciones
    /// </summary>
    public class Cotizacion
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int ID_Cotizacion { get; set; }
        /// <summary>
        /// Id del usuario que ha solicitado la solicitud
        /// </summary>
        public string? Id_Solicitante { get; set; }
        public int? IdS { get; set; }
        /// <summary>
        /// Guarda la fecha de creacion
        /// </summary>
        public DateTime? Fecha_Creacion_Cotizacion { get; set; }
        /// <summary>
        /// Etapa en la que se encuentra la cotizacion
        /// </summary>
        public string? Estado { get; set; }
        /// <summary>
        /// Id del proveedor asociado
        /// </summary>
        public string? Bien_Servicio { get; set; }
        public int Id_Bien_Servicio { get; set; }
        /// <summary>
        /// descripcion de la cotizacion
        /// </summary>
        public string? Detalle { get; set; }
        /// <summary>
        /// Numero de solped
        /// </summary>
        public long? Solped {  get; set; }

        public bool Activado { get; set; }


    }
}
