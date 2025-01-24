using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    /// <summary>
    /// Guarda los nombres de los bienes y servicios
    /// </summary>
    public class BienServicio
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key] 
        public int ID_Bien_Servicio {  get; set; }
        /// <summary>
        /// Nombre del bien o servicio
        /// </summary>
        public string? Bien_Servicio { get; set;}

        public bool Activado { get; set; }

    }
}
