using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    /// <summary>
    /// Guarda los centros de costos
    /// </summary>
    public class CentroCosto
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int Id_Ceco { get; set; }
        /// <summary>
        /// Codigo asociado a cada Centro de costo
        /// </summary>
        public string? Codigo_Ceco { get; set; }
        /// <summary>
        /// Nombre del centro de costo
        /// </summary>
        public string? Nombre { get; set; }

        public bool Activado { get;set; }
        
    }
}
