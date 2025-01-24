using System.ComponentModel.DataAnnotations;

namespace BaseDatosTPC
{
    /// <summary>
    /// Clase que guarda los datos de los departamentos
    /// </summary>
    public class Departamento
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int Id_Departamento { get; set; }
        /// <summary>
        /// Nombre del departamento
        /// </summary>
        public string? Nombre { get; set; }
        /// <summary>
        /// descripcion sobre el departamento
        /// </summary>
        public string? Descripcion {get;set;}
        /// <summary>
        /// Persona responsable del departamento
        /// </summary>
        public string? Encargado { get; set; }
        public int? Id_Encargado { get; set; }
        public bool Activado { get; set;}

    
    }
}
